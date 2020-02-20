import { Component, OnInit } from '@angular/core';
import { UserService } from 'src/app/_services/user.service';
import { AlertifyService } from 'src/app/_services/alertify.service';
import { ActivatedRoute } from '@angular/router';
import { User } from 'src/app/_models/User';

@Component({
  selector: 'app-member-detail',
  templateUrl: './member-detail.component.html',
  styleUrls: ['./member-detail.component.css']
})
export class MemberDetailComponent implements OnInit {
  user:User;
  constructor(private userService: UserService, private alertifyService: AlertifyService,
              private route: ActivatedRoute) { }

  ngOnInit() {
    this.loadUser();
  }

  // members/2
  loadUser() {
    const id = this.route.snapshot.params['id'];
    this.userService.getUser(+id).subscribe((user:User) => { this.user = user }, 
      error => this.alertifyService.error(error));
  }

}
