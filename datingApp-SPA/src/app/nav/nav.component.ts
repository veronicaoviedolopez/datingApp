import { Component, OnInit, Input } from '@angular/core';
import { AuthService } from '../_services/auth.service';
import { AlertifyService } from '../_services/alertify.service';
import { Router } from '@angular/router';

@Component({
  selector: 'app-nav',
  templateUrl: './nav.component.html',
  styleUrls: ['./nav.component.css']
})
export class NavComponent implements OnInit {
  model: any = {};
  photoUrl: string;

  constructor(public authService: AuthService,
    private alertifyService: AlertifyService,
    private router: Router) { }

  ngOnInit() {
    this.authService.currentPhotoUrl.subscribe(
        photo => {
          this.photoUrl = photo,
          console.log('this.currentUser.photoUrl', photo);
        },
        error => this.alertifyService.error(error));
  }

  Login() {
    this.authService.login(this.model)
    .subscribe(() => this.alertifyService.success('logged in succesfully'),
    error =>  this.alertifyService.error(error),
    () => this.router.navigate(['/members']))
  }

  LoggedIn() {
    return this.authService.loggedIn();
  }

  LoggOut() {
    localStorage.removeItem('token');
    localStorage.removeItem('user');
    this.authService.currentUser = null;
    this.authService.decodedToken= null;
    this.alertifyService.message('logged out');
    this.router.navigate(['/home']);
  }
}
