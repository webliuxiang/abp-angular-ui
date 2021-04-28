import { Directive, forwardRef, Attribute, Input } from '@angular/core';
import { Validator, AbstractControl, NG_VALIDATORS } from '@angular/forms';

@Directive({
    selector: '[expression]',
    providers: [
        {
            provide: NG_VALIDATORS,
            useExisting: forwardRef(() => ExpressionValidator),
            multi: true,
        },
    ],
})
export class ExpressionValidator implements Validator {
    @Input('expression')
    expression: string[] | string;

    validate(control: AbstractControl): { [key: string]: any } {
        let validationResult = null;

        if (!this.expression) {
            return validationResult;
        }

        if (!control.value || control.value.length < 0) {
            return validationResult;
        }

        const currentValue = control.value;


        let error = false;
        let regexp: RegExp;
        if (Array.isArray(this.expression)) {

            for (let index = 0; index < this.expression.length; index++) {
                regexp = new RegExp(this.expression[index]);
                if (!regexp.test(currentValue)) {
                    error = true;
                    break;
                }
            }
        } else {
            regexp = new RegExp(this.expression);
            error = !regexp.test(currentValue);
        }


        if (error) {
            validationResult = {};
            validationResult.expression = true;
        } else {
            validationResult = null;
        }

        return validationResult;
    }
}
