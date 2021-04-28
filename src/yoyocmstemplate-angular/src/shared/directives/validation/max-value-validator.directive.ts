import { Directive, forwardRef, Attribute, Input } from '@angular/core';
import { Validator, AbstractControl, NG_VALIDATORS } from '@angular/forms';

@Directive({
  selector: '[maxValue]',
  providers: [
    {
      provide: NG_VALIDATORS,
      useExisting: forwardRef(() => MaxValueValidator),
      multi: true,
    },
  ],
})
export class MaxValueValidator implements Validator {
  @Input('maxValue')
  maxValue: number;

  validate(control: AbstractControl): { [key: string]: any } {
    const currentValue = control.value;
    let validationResult = null;

    if (!currentValue) { return validationResult; }

    const maxValue = this.maxValue;
    if (maxValue && currentValue > maxValue) {
      validationResult = validationResult || {};
      validationResult.maxValue = true;
    }

    return validationResult;
  }
}
