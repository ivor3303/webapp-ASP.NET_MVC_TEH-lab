using System.Globalization;
using Microsoft.AspNetCore.Mvc.ModelBinding;

namespace Vjezba.App.ModelBinding
{
    public sealed class CommaOrDotDecimalModelBinderProvider : IModelBinderProvider
    {
        public IModelBinder? GetBinder(ModelBinderProviderContext context)
        {
            if (context.Metadata.ModelType == typeof(decimal) || context.Metadata.ModelType == typeof(decimal?))
            {
                return new CommaOrDotDecimalModelBinder();
            }

            return null;
        }
    }

    public sealed class CommaOrDotDecimalModelBinder : IModelBinder
    {
        public Task BindModelAsync(ModelBindingContext bindingContext)
        {
            ArgumentNullException.ThrowIfNull(bindingContext);

            var valueResult = bindingContext.ValueProvider.GetValue(bindingContext.ModelName);
            if (valueResult == ValueProviderResult.None)
            {
                return Task.CompletedTask;
            }

            bindingContext.ModelState.SetModelValue(bindingContext.ModelName, valueResult);

            var rawValue = valueResult.FirstValue?.Trim();
            if (string.IsNullOrEmpty(rawValue))
            {
                if (bindingContext.ModelMetadata.IsReferenceOrNullableType)
                {
                    bindingContext.Result = ModelBindingResult.Success(null);
                }
                else
                {
                    bindingContext.ModelState.TryAddModelError(bindingContext.ModelName, $"Polje {bindingContext.ModelName} je obavezno.");
                }

                return Task.CompletedTask;
            }

            if (TryParseDecimal(rawValue, out var value))
            {
                bindingContext.Result = ModelBindingResult.Success(value);
            }
            else
            {
                bindingContext.ModelState.TryAddModelError(bindingContext.ModelName, $"Vrijednost '{rawValue}' nije valjan decimalni broj.");
            }

            return Task.CompletedTask;
        }

        private static bool TryParseDecimal(string rawValue, out decimal value)
        {
            var normalized = rawValue;

            var lastComma = normalized.LastIndexOf(',');
            var lastDot = normalized.LastIndexOf('.');

            if (lastComma >= 0 && lastDot >= 0)
            {
                if (lastComma > lastDot)
                {
                    normalized = normalized.Replace(".", string.Empty).Replace(',', '.');
                }
                else
                {
                    normalized = normalized.Replace(",", string.Empty);
                }
            }
            else if (lastComma >= 0)
            {
                normalized = normalized.Replace(',', '.');
            }

            return decimal.TryParse(normalized, NumberStyles.Number, CultureInfo.InvariantCulture, out value);
        }
    }
}