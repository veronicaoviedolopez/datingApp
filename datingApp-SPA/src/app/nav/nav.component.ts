import { Component, OnInit, Input } from '@angular/core';
import { AuthService } from '../_services/auth.service';
import { AlertifyService } from '../_services/alertify.service';

@Component({
  selector: 'app-nav',
  templateUrl: './nav.component.html',
  styleUrls: ['./nav.component.css']
})
export class NavComponent implements OnInit {
  model: any = {};
  token_decoded: any;

  constructor(private authService: AuthService,
    private alertifyService: AlertifyService) { }

  ngOnInit() {
  }

  Login() {
    this.authService.login(this.model)
    .subscribe(response => {
      this.token_decoded = response;
      console.log(this.token_decoded);
      this.alertifyService.success('logged in succesfully');
    }, 
    error =>  this.alertifyService.error(error))
  }

  LoggedIn() {
    return this.authService.loggedIn();
  }

  readToken() {
    return this.authService.readToken(localStorage.getItem('token')).unique_name;
  }

  LoggOut() {
    localStorage.removeItem('token');
    this.alertifyService.message('logged out')
  }
}
