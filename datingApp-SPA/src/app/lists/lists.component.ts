import { Component, OnInit } from '@angular/core';
import { UserService } from '../_services/user.service';
import { ActivatedRoute } from '@angular/router';
import { AlertifyService } from '../_services/alertify.service';
import { User } from '../_models/User';
import { Pagination, PaginatedResult } from '../_models/Pagination';
import { AuthService } from '../_services/auth.service';

@Component({
  selector: 'app-lists',
  templateUrl: './lists.component.html',
  styleUrls: ['./lists.component.css']
})
export class ListsComponent implements OnInit {
  users: User[];
  pagination: Pagination;
  likesParam = 'Likers';

  constructor(private authService: AuthService, private userService: UserService,
      private alertifyService: AlertifyService, private route: ActivatedRoute) { }

  ngOnInit() {
    this.route.data.subscribe(data => {
      this.users = data.user.result;
      this.pagination = data.user.pagination;
    });
  }

  loadUsers() {
    this.userService.getUsers(this.pagination.currentPage,
      this.pagination.itemsPerPage, null, this.likesParam)
      .subscribe(res => {
        this.users = res.result;
        this.pagination = res.pagination;
      }, error => this.alertifyService.error(error));
  }

  pageChanged({page, itemsPerPage}) {
    this.pagination.currentPage = page;
    this.loadUsers();
  }

}
