using Microsoft.AspNetCore.Mvc.DataAnnotations;
using Microsoft.AspNetCore.Mvc.ModelBinding.Validation;
using Microsoft.Extensions.Localization;
using System.Collections;
using System.ComponentModel.DataAnnotations;

namespace PUTSWeb.Helpers
{
    public class MinimumCount : ValidationAttribute
    {
        private readonly int minimumElements;

        public MinimumCount(int min)
        {
            minimumElements = min;
        }

        public override bool IsValid(object value)
        {
            var collection = value as ICollection;
            if (collection != null)
            {
                return collection.Count >= minimumElements;
            }

            return false;
        }
    }

    public class MinimumCountAdapter : AttributeAdapterBase<MinimumCount>
    {
        public MinimumCountAdapter(MinimumCount attribute, IStringLocalizer stringLocalizer) : base(attribute, stringLocalizer) { }

        public override void AddValidation(ClientModelValidationContext context) { }

        public override string GetErrorMessage(ModelValidationContextBase validationContext)
        {
            return GetErrorMessage(validationContext.ModelMetadata, validationContext.ModelMetadata.GetDisplayName());
        }
    }

    public class CustomValidationAttributeAdapterProvider : IValidationAttributeAdapterProvider
    {
        private readonly IValidationAttributeAdapterProvider _baseProvider = new ValidationAttributeAdapterProvider();

        public IAttributeAdapter GetAttributeAdapter(ValidationAttribute attribute, IStringLocalizer stringLocalizer)
        {
            if (attribute is MinimumCount)
                return new MinimumCountAdapter(attribute as MinimumCount, stringLocalizer);
            else if (attribute is SourceFileValidation)
                return new SourceFileValidationAdapter(attribute as SourceFileValidation, stringLocalizer);
            else
                return _baseProvider.GetAttributeAdapter(attribute, stringLocalizer);
        }
    }
}
