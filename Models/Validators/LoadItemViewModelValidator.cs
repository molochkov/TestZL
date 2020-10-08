using FluentValidation;
using FluentValidation.Validators;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Http;
using System;
using System.Collections.Generic;
using System.IO;
using System.IO.Compression;
using System.Linq;
using System.Text.RegularExpressions;
using System.Threading.Tasks;
using TestZL.DAL;
using TestZL.Helpers;
using TestZL.Models.DTO;
using TestZL.Models.ViewModel;
using TestZL.Services;
using TestZL.Services.Interfaces;

namespace TestZL.Models.Validators
{
    public class LoadItemViewModelValidator : AbstractValidator<LoadItemViewModel>
    {
        private readonly IWebHostEnvironment appEnvironment;
        private readonly ConfigurationHelper сonfigurationHelper;
        private readonly IFileService fileService;
        private readonly TestZLContext contextDb;


        private string TempFilePath = string.Empty;


        public LoadItemViewModelValidator(
            IWebHostEnvironment appEnvironment,
            ConfigurationHelper сonfigurationHelper,
            IFileService fileService,
            TestZLContext context
            )
        {

            this.appEnvironment = appEnvironment;
            this.сonfigurationHelper = сonfigurationHelper;
            this.fileService = fileService;
            this.contextDb = context;

            this.RuleFor(x => x.item.Name)
                .Cascade(CascadeMode.StopOnFirstFailure)
                .NotNull().WithMessage("Значение не может быть null")
                .NotEmpty().WithMessage("Значение не может быть пустым")
                .MaximumLength(8).WithMessage("Максимальная длина имени 8 символов")
                .MinimumLength(4).WithMessage("Минимальная длина имени 4 символа")
                .Must((Name) => { return IsValidCaptcha(Name); }).WithMessage("Имя не может содержать слово 'captcha'")
                .Must((Name) => { return IsValidLatin(Name); }).WithMessage("Имя должно содержать только латинские буквы");


            this.RuleFor(x => x.item.SelectedMarkupId)
                .Cascade(CascadeMode.StopOnFirstFailure)
            .NotNull().WithMessage("Необходимо выбрать одно из значений")
            .NotEmpty().WithMessage("Необходимо выбрать одно из значений");


            this.RuleFor(x => new { x.item.IsContainsCyrillic, x.item.IsContainsLatin, x.item.IsContainsDigits })
                .Cascade(CascadeMode.StopOnFirstFailure)
                .Must(x => { return IsValidChecked(x.IsContainsCyrillic, x.IsContainsLatin, x.IsContainsDigits); })
                .WithMessage("Необходимо выбрать минимум одно из: “Содержит кириллицу”, “Содержит латиницу”, “Содержит цифры”");
            this.RuleFor(x => x.uploadedFile)
                .NotNull().WithMessage("Необходимо прикрепить файл");


            this.RuleFor(x => new { x.item, x.uploadedFile })
                .Cascade(CascadeMode.StopOnFirstFailure)
                .Must(x => { return IsNullFile(x.item, x.uploadedFile); }).WithMessage("прикрепите файл")
                .Custom((model, context) =>
                {
                    ValidateFile(model.item, model.uploadedFile, context);
                });
        }

        /// <summary>
        /// Валидация на слово capcha
        /// </summary>
        /// <param name="inStr"></param>
        /// <returns></returns>
        bool IsValidCaptcha(string inStr)
        {
            Boolean result = true;
            if (inStr.Contains("capcha", StringComparison.OrdinalIgnoreCase))
            {
                result = false;
            }
            return result;
        }

        /// <summary>
        /// Валидация на латиницу
        /// </summary>
        /// <param name="inStr"></param>
        /// <returns></returns>
        bool IsValidLatin(string inStr)
        {
            Boolean result = false;
            if (Regex.IsMatch(inStr, "^[a-zA-Z]*$"))
            {
                result = true;
            }
            return result;
        }
        /// <summary>
        /// Валидация на выбор одного из трех параметров
        /// </summary>
        /// <param name="IsContainsCyrillic"></param>
        /// <param name="IsContainsLatin"></param>
        /// <param name="IsContainsDigits"></param>
        /// <returns></returns>
        bool IsValidChecked(bool IsContainsCyrillic, bool IsContainsLatin, bool IsContainsDigits)
        {
            Boolean result = false;
            if (IsContainsCyrillic || IsContainsLatin || IsContainsDigits)
            {
                result = true;
            }
            return result;
        }

        /// <summary>
        /// Кастомная валидация загружаемого файла
        /// </summary>
        /// <param name="model"></param>
        /// <param name="file"></param>
        /// <param name="context"></param>
        private void ValidateFile(LoadItemWithOutFile model, IFormFile file, CustomContext context)
        {
            //определяем границы минимального и максимального значения числа файлов
            int MinCount = сonfigurationHelper.MinFilesInZip();
            int MaxCount = сonfigurationHelper.MaxFilesInZip();

            // на сколько надо изменить границы
            int CountChangeLimit = сonfigurationHelper.UpLimit() * (
              Convert.ToInt32(model.IsContainsLatin) +
              Convert.ToInt32(model.IsContainsDigits) +
              Convert.ToInt32(model.IsContainsSpecialChars) +
              Convert.ToInt32(model.IsCaseSensitivity));
            MinCount += CountChangeLimit;
            MaxCount += CountChangeLimit;
            try
            {
                using (ZipArchive archive = ZipFile.OpenRead(TempFilePath))
                {
                    //определяем общее кол-во файлов
                    int countAllFile = archive.Entries.Count();

                    //определяем кол-во  файлов разметки в архиве
                    int CountFileAnswer = archive.Entries.Count(x => x.Name.Equals(сonfigurationHelper.AnswerFileName()));

                    //определяем кол-во  файлов с картинками
                    var CountImageFile = archive.Entries.Count(x => сonfigurationHelper.AllowImageFormat().Any(y => x.Name.Contains(y)));

                    //получаем из базы выбранную запись 'Расположение разметки'
                    var LocationMarkup = this.contextDb.LocationMarkups.Where(x => x.Id == new Guid(model.SelectedMarkupId)).FirstOrDefault();

                    // если режим Расположение разметки в отдельном файле
                    if (LocationMarkup.Syn.Equals(сonfigurationHelper.SynAnswerInSeparateFile(), StringComparison.OrdinalIgnoreCase))
                    {
                        if (CountFileAnswer == 0)
                        {
                            context.AddFailure($"В загружаемом архиве отсутствует файл с ответами");
                            return;
                        }
                        if (CountFileAnswer > 1)
                        {
                            context.AddFailure($"В загружаемом ,больше 1 файла с ответами");
                            return;
                        }

                        // читаем файл с ответами

                        var AnswerFile = archive.GetEntry(сonfigurationHelper.AnswerFileName());
                        using (StreamReader fstream = new StreamReader(AnswerFile.Open()))
                        {
                            IEnumerable<AnswerModel> answers = fstream.ReadToEnd()
                                .Split("\n")
                                .Where(x => x.Length > 0 && x.Contains(":"))
                                .Select(x => new AnswerModel() { FileName = x.Split(':')[0], Answer = x.Split(':')[1] });

                            //Получаем кол-во файлов у которых в файле ответов найден ответ
                            int CountCheckAnswer = archive.Entries
                                .Where(x => сonfigurationHelper.AllowImageFormat().Any(y => x.Name.Contains(y)))
                                .Join(answers, x => x.Name, y => y.FileName, (x, y) => new { x.Name, y.Answer }).Count();

                            // Сравниваем кол-во файлов с ответами с общим кол-вом графических файлов в архиве
                            if (CountImageFile != CountCheckAnswer)
                            {
                                context.AddFailure($"В загружаемом архиве не совпадает кол-во графических файлов {CountImageFile}, с кол-вом ответов {CountCheckAnswer}");
                                return;
                            }
                        }
                    }

                    //проверка на кол-во графических файлов
                    if (CountImageFile < MinCount || CountImageFile > MaxCount)
                    {
                        context.AddFailure($"В загружаемом архиве {archive.Entries.Count()} файлов. Допустимое кол-во файлов {MinCount}-{MaxCount}");
                        return;
                    }

                }
            }
            catch (Exception e)
            {
                context.AddFailure("Ошибка чтения файла");
            }
        }

        private bool IsNullFile(LoadItemWithOutFile model, IFormFile file)
        {
            Boolean result = false;
            if (file != null)
            {
                // создаем временную папку и загружаем туда файл           
                string path = fileService.GetPathToTempFileDirectory(model.TempFolder);
                TempFilePath = this.fileService.UploadFileAsync(path, file).GetAwaiter().GetResult();
                result = true;
            }
            return result;

        }
    }
}
