import { Directive, forwardRef, Attribute, Input } from '@angular/core';
import { Validator, AbstractControl, NG_VALIDATORS } from '@angular/forms';

@Directive({
  selector: '[minValue]',
  providers: [
    {
      provide: NG_VALIDATORS,
      useExisting: forwardRef(() => MinValueValidator),
      multi: true,
    },
  ],
})
export class MinValueValidator implements Validator {
  @Input('minValue')
  minValue: number;

  validate(control: AbstractControl): { [key: string]: any } {
    const currentValue = control.value;
    let validationResult = null;

    if (!currentValue) {
      return validationResult;
    }



    const minValue = this.minValue;
    if (minValue && currentValue < minValue) {
      validationResult = validationResult || {};
      validationResult.minValue = true;
    }

    return validationResult;
  }
}
