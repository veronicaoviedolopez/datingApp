import { Component, OnInit, Input } from '@angular/core';
import { Message } from 'src/app/_models/Message';
import { UserService } from 'src/app/_services/user.service';
import { AuthService } from 'src/app/_services/auth.service';
import { AlertifyService } from 'src/app/_services/alertify.service';
import { error } from 'protractor';
import { tap } from 'rxjs/operators';

@Component({
  selector: 'app-member-messages',
  templateUrl: './member-messages.component.html',
  styleUrls: ['./member-messages.component.css']
})
export class MemberMessagesComponent implements OnInit {
  @Input() recipientId: number;
  messages: Message[];
  newMessage: any = {};

  constructor(private  userService: UserService, private authService: AuthService,
    private alertifyService: AlertifyService) { }

  ngOnInit() {
    this.loadMessages();
  }

  loadMessages() {
    const currentUserId = +this.authService.decodedToken.nameid;
    this.userService.getMessageThread(currentUserId, this.recipientId)
    .pipe(
      tap(messages => {
        messages.forEach(message => {
          if (message.isRead===false && message.recipientId === currentUserId) {
            this.userService.markAsRead(currentUserId, message.id);}
        });
      })
    )
    .subscribe(messages => this.messages = messages,
      error => this.alertifyService.error(error));
  }

  sendMessage() {
    this.newMessage.recipientId = this.recipientId;
    this.newMessage.senderId = this.authService.decodedToken.nameid;
    this.userService.sendMessage(this.authService.decodedToken.nameid, this.newMessage)
      .subscribe((message:Message) => {
        this.messages.unshift(message);
        this.newMessage.content = '';
      }, error => this.alertifyService.error(error));
  }

}
