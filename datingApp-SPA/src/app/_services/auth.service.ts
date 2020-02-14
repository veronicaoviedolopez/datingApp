import { Injectable } from '@angular/core';
import { HttpClient } from '@angular/common/http';
import { map } from 'rxjs/operators';
import { JwtHelperService } from "@auth0/angular-jwt";

@Injectable({
  providedIn: 'root'
})
export class AuthService {
  private baseUrl = 'http://localhost:5000/api/auth/';
  private helper = new JwtHelperService();
  
  constructor(private http: HttpClient) {}

  login(model: any) {
    return this.http.post(`${this.baseUrl}login`, model).pipe(
      map((response: any) => {
        const user = response;
        if (user) {
          localStorage.setItem('token', user.token);
          return this.readToken(user.token);
        }
      })
    );
  }

  readToken(token?: any) {
   return this.helper.decodeToken(token);
    //this.expirationDate = this.helper.getTokenExpirationDate(token);
  }

  register(model: any) {
    return this.http.post(`${this.baseUrl}`, model);
  }

  loggedIn() {
    const token = localStorage.getItem('token');
    const isExpired = this.helper.isTokenExpired(token);
    return !isExpired;
  }
}
