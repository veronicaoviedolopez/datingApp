import { Component, OnInit, ViewChild, Host, HostListener } from '@angular/core';
import { ActivatedRoute } from '@angular/router';
import { User } from 'src/app/_models/User';
import { AlertifyService } from 'src/app/_services/alertify.service';
import { NgForm } from '@angular/forms';
import { UserService } from 'src/app/_services/user.service';
import { AuthService } from 'src/app/_services/auth.service';

@Component({
  selector: 'app-member-edit',
  templateUrl: './member-edit.component.html',
  styleUrls: ['./member-edit.component.css']
})
export class MemberEditComponent implements OnInit {
  @ViewChild('editForm', {static:true}) editForm: NgForm;
  user: User;
  @HostListener('window:beforeunload', ['$event'])
  unloadNotification($event:any) {
    if(this.editForm.dirty) {
      $event.returnValue = true;
    }
  }

  constructor(private route: ActivatedRoute, private alertify: AlertifyService,
        private userService: UserService, private authService: AuthService) { }

  ngOnInit() {
    this.route.data.subscribe(data => this.user = data['user'],
    error => this.alertify.error(error));
  }

  updateUser() {
    const id = this.authService.readToken().nameid;
    this.userService.updateUser(id, this.user).subscribe(() => {
      this.alertify.success('Profile updated successfully');
      this.editForm.reset(this.user);
    }, error => this.alertify.error(error));
  }
}
