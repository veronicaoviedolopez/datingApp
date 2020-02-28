import { Component, OnInit, ViewChild, Host, HostListener } from '@angular/core';
import { ActivatedRoute } from '@angular/router';
import { User } from 'src/app/_models/User';
import { AlertifyService } from 'src/app/_services/alertify.service';
import { NgForm } from '@angular/forms';

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

  constructor(private route: ActivatedRoute, private alertifyService: AlertifyService,
        private alertify: AlertifyService) { }

  ngOnInit() {
    this.route.data.subscribe(data => this.user = data['user'],
    error => this.alertifyService.error(error));
  }

  updateUser() {
    this.alertify.success('Profile updated successfully');
    this.editForm.reset(this.user);
  }
}
