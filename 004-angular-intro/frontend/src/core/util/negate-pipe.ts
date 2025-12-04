import { Pipe, PipeTransform } from '@angular/core';

@Pipe({
  name: 'negate',
})
export class NegatePipe implements PipeTransform {

  transform(value: boolean, ...args: unknown[]): unknown {
    return !value;
  }

}
