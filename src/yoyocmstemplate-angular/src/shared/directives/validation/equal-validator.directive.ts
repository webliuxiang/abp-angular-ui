import { Directive, forwardRef, Attribute, Input } from '@angular/core';
import { Validator, AbstractControl, NG_VALIDATORS, ValidationErrors } from '@angular/forms';

// Got from: https://scotch.io/tutorials/how-to-implement-a-custom-validator-directive-confirm-password-in-angular-2

@Directive({
  // tslint:disable-next-line:max-line-length
  selector: '[validateEqual][formControlName],[validateEqual][formControl],[validateEqual][ngModel],:not([type=checkbox])[confirm][formControlName],:not([type=checkbox])[confirm][formControl],:not([type=checkbox])[confirm][ngModel]',
  providers: [
    {
      provide: NG_VALIDATORS,
      useExisting: forwardRef(() => EqualValidator),
      multi: true,
    },
  ],
})
export class EqualValidator implements Validator {

  private _default = {};
  private _enabled = false;
  private _onChange?: () => void;
  private _confirmValue: string;

  constructor() {
  }

  @Input()
  set confirm(value: string) {
    this._confirmValue = value;
    this._enabled = typeof (value) === 'string';
    if (this._onChange) {
      this._onChange();
    }
  }

  @Input()
  set validateEqual(value: string) {
    this.confirm = value;
  }

  validate(control: AbstractControl): ValidationErrors | null {
    if (!this._enabled) {
      return this._default;
    }
    if (!control.value || control.value.trim() === '') {
      return this._default;
    }
    if (control.value !== this._confirmValue) {
      return { validateEqual: true, confirm: true, error: true };
    }

    return this._default;
  }

  registerOnValidatorChange(fn: () => void): void {
    this._onChange = fn;
  }
}
