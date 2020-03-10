import { Component, OnInit, Input, Output, EventEmitter } from '@angular/core';
import { AuthService } from '../_services/auth.service';
import { AlertifyService } from '../_services/alertify.service';
import { FormGroup, Validators, FormBuilder } from '@angular/forms';
import { BsDatepickerConfig } from 'ngx-bootstrap/datepicker';
import { Router } from '@angular/router';

@Component({
  selector: 'app-register',
  templateUrl: './register.component.html',
  styleUrls: ['./register.component.css']
})
export class RegisterComponent implements OnInit {
  model: any = {}
  registerForm: FormGroup;
  colorTheme = 'theme-red';
  bsConfig: Partial<BsDatepickerConfig>;

  @Output() cancelRegister = new EventEmitter();
  user: any;
  constructor(private authService:AuthService,
    private alertifyService: AlertifyService,
    private formBuilder: FormBuilder,
    private router: Router) { }

  ngOnInit() {
    this.bsConfig = Object.assign({}, { containerClass: this.colorTheme,  isAnimated: true  });
    this.createRegisterForm();
  }

  createRegisterForm(){
    this.registerForm = this.formBuilder.group({
      username: ['', Validators.required],
      password: ['', [Validators.required, Validators.minLength(4), Validators.maxLength(8)]],
      confirmPassword: ['', [Validators.required, Validators.minLength(4), Validators.maxLength(8)]],
      knownAs: ['', Validators.required],
      dateOfBirth: ['', Validators.required],
      city: ['', Validators.required],
      country: ['', Validators.required],
      gender:['', Validators.required]

    }, {validator: this.passwordMatchValidator});
  }

  passwordMatchValidator(form: FormGroup){
    return form.get('password').value === form.get('confirmPassword').value ? null : {mistach: true};
  }

  register() {
    if(this.registerForm.valid) {
      this.user = Object.assign({}, this.registerForm.value);
      this.authService.register(this.user).subscribe(x=>
          this.alertifyService.success('registration successful'),
          error =>  this.alertifyService.error(error),
          () => this.authService.login(this.user).subscribe(() =>
              this.router.navigate(['/members']))
      );
    }
  }

  cancel() {
    this.cancelRegister.emit(false);
  }

}
