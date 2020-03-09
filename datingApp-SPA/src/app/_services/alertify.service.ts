import { Injectable } from '@angular/core';
import  * as alertify from 'alertifyjs';
alertify.defaults.glossary.title = 'Confirmation';
@Injectable({
  providedIn: 'root'
})
export class AlertifyService {

constructor() { }

confirm(message: string, okCallback: () => any) {
  alertify.confirm(message, (e:any) => {
    if(e) {
      okCallback();
    }
  })
}
P
success(message: string) {
  alertify.success(message);
}

error(message: string) {
  alertify.error(message);
}

warning(message: string) {
  alertify.warning(message);
}

message(message: string) {
  alertify.message(message);
}


}
