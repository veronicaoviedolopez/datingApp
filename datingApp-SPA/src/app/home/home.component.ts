import { Component, OnInit } from '@angular/core';
import { HttpClient } from '@angular/common/http';

@Component({
  selector: 'app-home',
  templateUrl: './home.component.html',
  styleUrls: ['./home.component.css']
})
export class HomeComponent implements OnInit {
  registerMode = false;
  values: any;
  constructor(private http:HttpClient) { }

  ngOnInit() {
   // this.getData();
  }

  getData() {
    this.http.get('http://localhost:5000/api/values').subscribe( payload => this.values = payload, error => console.log(error));
  }

  registerToggle() {
    this.registerMode = true;;
  }

  cancelRegisterHome(registerMode:boolean){
    this.registerMode = registerMode;
  }

}
