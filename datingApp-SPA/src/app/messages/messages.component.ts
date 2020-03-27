import { Component, OnInit } from '@angular/core';
import { Pagination, PaginatedResult } from '../_models/Pagination';
import { Message } from '../_models/Message';
import { UserService } from '../_services/user.service';
import { AuthService } from '../_services/auth.service';
import { ActivatedRoute } from '@angular/router';
import { AlertifyService } from '../_services/alertify.service';
import { error } from 'protractor';

@Component({
  selector: 'app-messages',
  templateUrl: './messages.component.html',
  styleUrls: ['./messages.component.css']
})
export class MessagesComponent implements OnInit {
  messages: Message[];
  pagination: Pagination;
  messageContainer = 'Unread';
  
  constructor(private userSevice: UserService, private authService: AuthService,
    private route: ActivatedRoute, private alertify: AlertifyService) { }

  ngOnInit() {
    this.route.data.subscribe(data => {
      this.messages = data["messages"].result;
      this.pagination = data['messages'].pagination;
    });
  }

  loadMessages() {
    this.userSevice.getMessages(this.authService.decodedToken.nameid, 
      this.pagination.currentPage, this.pagination.itemsPerPage, this.messageContainer)
      .subscribe( (res: PaginatedResult<Message[]>) => {
        this.messages = res.result;
        this.pagination = res.pagination;
      }, error => this.alertify.error(error))
  }

  pageChanged(event: any): void {
    this.pagination.currentPage = event.page;
    this.loadMessages();
  }

  deleteMessage(id: number, index: number) {
    console.log(index);
     this.alertify.confirm('Are you sure you want to delete this message?'
    ,() => this.userSevice.deleteMessage(this.authService.decodedToken.nameid, id)
    .subscribe((x=> {
      this.messages.splice(index, 1);
      this.alertify.success('message has been deleted');
    })
    , error => this.alertify.error(error))
    );
    
  }

}
