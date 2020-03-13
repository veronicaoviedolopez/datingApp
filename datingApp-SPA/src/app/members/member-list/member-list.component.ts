import { Component, OnInit } from '@angular/core';
import { UserService } from '../../_services/user.service';
import { AlertifyService } from '../../_services/alertify.service';
import { User } from '../../_models/User';
import { ActivatedRoute } from '@angular/router';
import { Pagination } from 'src/app/_models/Pagination';

@Component({
  selector: 'app-member-list',
  templateUrl: './member-list.component.html',
  styleUrls: ['./member-list.component.css']
})
export class MemberListComponent implements OnInit {
  users:User[];
  pagination: Pagination;
  user: User = JSON.parse(localStorage.getItem('user'));
  genderList = [{value:'male', display: 'Males'},{value:'female', display: 'Females'}]
  userParams :any = {};
  constructor(private userService:UserService, private alertifyService:AlertifyService,
              private route: ActivatedRoute) { }

  ngOnInit() {
    this.resetUserParams();
    this.route.data.subscribe(data => {
      this.users = data['users'].result;
      this.pagination = data['users'].pagination;
    });
  }

  resetFilters(){
    this.resetUserParams();
    this.loadUsers();
  }

  private resetUserParams() {
    this.userParams.gender = this.user.gender === 'male' ? 'female' : 'male';
    this.userParams.minAge = 18;
    this.userParams.maxAge = 99;
    this.userParams.orderBy = 'lastActive';
  }

  pageChanged({page, itemsPerPage}) {
    this.pagination.currentPage = page;
    this.loadUsers();
  }
  loadUsers() {
    this.userService.getUsers(this.pagination.currentPage,
      this.pagination.itemsPerPage, {...this.userParams, orderBy:this.userParams.orderBy})
      .subscribe(res => {
        this.users = res.result;
        this.pagination = res.pagination;
      }, error => this.alertifyService.error(error));
  }
}
