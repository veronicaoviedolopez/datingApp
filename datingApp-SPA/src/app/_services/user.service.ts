import { Injectable } from "@angular/core";
import { environment } from "src/environments/environment";
import { HttpClient, HttpHeaders } from '@angular/common/http';
import { Observable } from 'rxjs';
import { User } from '../_models/User';
import { map } from 'rxjs/operators';

@Injectable({
  providedIn: "root"
})
export class UserService {
  private baseUrl:string = environment.apiUrl + "user/";
  constructor(private http: HttpClient) {}

  getUsers(): Observable<User[]> {
    // return this.http.get(this.baseUrl).pipe(map((p:User[]) => p));
    return this.http.get<User[]>(this.baseUrl);
  }

  getUser(id): Observable<User> {
    return this.http.get<User>(this.baseUrl + id);
  }
}
