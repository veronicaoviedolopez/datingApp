import { Component, OnInit, Input } from '@angular/core';
import { User } from 'src/app/_models/User';
import { UserService } from 'src/app/_services/user.service';
import { AlertifyService } from 'src/app/_services/alertify.service';
import { AuthService } from 'src/app/_services/auth.service';

@Component({
  selector: 'app-member-card',
  templateUrl: './member-card.component.html',
  styleUrls: ['./member-card.component.css']
})
export class MemberCardComponent implements OnInit {
  @Input() user: User;
  constructor(private userService: UserService, private authService: AuthService, private alertifyService: AlertifyService) { }

  ngOnInit() {
  }

  setLike() {
    this.userService.sendLike(this.authService.currentUser.id, this.user.id)
      .subscribe(() => this.alertifyService.success('Like succesfully'),
      (e) => this.alertifyService.error(e))
  }
}
