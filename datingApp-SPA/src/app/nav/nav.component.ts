import { Component, OnInit } from '@angular/core';
import { AuthService } from '../_services/auth.service';

@Component({
  selector: 'app-nav',
  templateUrl: './nav.component.html',
  styleUrls: ['./nav.component.css']
})
export class NavComponent implements OnInit {
  model: any = {};
  constructor(private authService: AuthService) { }

  ngOnInit() {
  }

  Login() {
    this.authService.login(this.model)
    .subscribe(next =>
      console.log('Logged in succesfully')
      , error => console.log('Failed to login', error))
  }

  LoggedIn() {
    const token = localStorage.getItem('token');
    return !!token;
  }

  LoggOut() {
    localStorage.removeItem('token');
    console.log('logged out');
  }
}
