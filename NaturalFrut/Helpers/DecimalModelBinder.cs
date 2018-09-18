using System;
using System.Globalization;
using System.Web.Mvc;

namespace Naturalfrut.Helpers
{

    public abstract class FloatingPointModelBinderBase<T> : DefaultModelBinder
    {
        protected abstract Func<string, IFormatProvider, T> ConvertFunc { get; }

        //public override object BindModel(ControllerContext controllerContext, ModelBindingContext bindingContext)
        //{
        //    var valueProviderResult = bindingContext.ValueProvider.GetValue(bindingContext.ModelName);
        //    if (valueProviderResult == null) return base.BindModel(controllerContext, bindingContext);
        //    try
        //    {
        //        return ConvertFunc.Invoke(valueProviderResult.AttemptedValue, CultureInfo.CurrentUICulture);
        //    }
        //    catch (FormatException)
        //    {
        //        // If format error then fallback to InvariantCulture instead of current UI culture
        //        return ConvertFunc.Invoke(valueProviderResult.AttemptedValue, CultureInfo.InvariantCulture);
        //    }
        //}

        public override object BindModel(ControllerContext controllerContext,
        ModelBindingContext bindingContext)
        {
            ValueProviderResult valueResult = bindingContext.ValueProvider
                .GetValue(bindingContext.ModelName);
            ModelState modelState = new ModelState { Value = valueResult };
            object actualValue = null;
            try
            {
                actualValue = Convert.ToDecimal(valueResult.AttemptedValue,
                    CultureInfo.CurrentCulture);
            }
            catch (FormatException e)
            {
                modelState.Errors.Add(e);
            }

            bindingContext.ModelState.Add(bindingContext.ModelName, modelState);
            return actualValue;
        }
    }

    public class DecimalModelBinder : FloatingPointModelBinderBase<decimal>
    {
        protected override Func<string, IFormatProvider, decimal> ConvertFunc => Convert.ToDecimal;
    }

    public class DoubleModelBinder : FloatingPointModelBinderBase<double>
    {
        protected override Func<string, IFormatProvider, double> ConvertFunc => Convert.ToDouble;
    }

    public class SingleModelBinder : FloatingPointModelBinderBase<float>
    {
        protected override Func<string, IFormatProvider, float> ConvertFunc => Convert.ToSingle;
    }
}