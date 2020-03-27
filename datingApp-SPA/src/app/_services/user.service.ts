import { Injectable } from '@angular/core';
import { environment } from 'src/environments/environment';
import { HttpClient, HttpHeaders, HttpParams } from '@angular/common/http';
import { Observable } from 'rxjs';
import { User } from '../_models/User';
import { map } from 'rxjs/operators';
import { PaginatedResult } from '../_models/Pagination';
import { Message } from '../_models/Message';

@Injectable({
  providedIn: 'root'
})
export class UserService {
  private baseUrl:string = environment.apiUrl;
  constructor(private http: HttpClient) {}

  getUsers(page?, itemsPerPage?, userParams?, likesParam?): Observable<PaginatedResult<User[]>> {
    const paginationResult: PaginatedResult<User[]> = new PaginatedResult<User[]>();
    let params = new HttpParams();
    if(page != null && itemsPerPage != null){
      params = params.append('pageNumber', page);
      params = params.append('pageSize', itemsPerPage);
    }

    if(userParams != null){
      params = params.append('gender', userParams.gender);
      params = params.append('minAge', userParams.minAge);
      params = params.append('maxAge', userParams.maxAge);
      params = params.append('orderBy', userParams.orderBy);
    }

    if(likesParam === 'Likers') {
      params = params.append('likers', 'true') }

    if(likesParam === 'Likees') {
      params = params.append('likees', 'true') }

    return this.http.get<User[]>(this.baseUrl+ 'user/', {observe:'response', params})
    .pipe(
      map(response => {
        paginationResult.result = response.body;
        if (response.headers.get('Pagination') != null) {
          paginationResult.pagination = JSON.parse(response.headers.get('Pagination'));
        }
        return paginationResult;
      })
    )
  }

  getUser(id): Observable<User> {
    return this.http.get<User>(this.baseUrl + 'user/' + id);
  }

  updateUser(id: number, user: User){
    return this.http.put(this.baseUrl  + 'user/' + id, user);
  }

  setMainPhoto(userId: number, photoId: number) {
    return this.http.post(this.baseUrl + 'user/' + userId + '/photos/' + photoId +'/setMain', {});
  }

  deletePhoto(userId: number, photoId:number) {
    return this.http.delete(`${this.baseUrl}user/${userId}/photos/${photoId}`);
  }

  sendLike(userId: number, userRecipient: number) {
    return this.http.post(`${this.baseUrl}user/${userId}/like/${userRecipient}`,{});
  }

  getMessages(userId: number, page?, itemsPerPage?, messageContainer?) {
    const paginatedResult:PaginatedResult<Message[]> = new PaginatedResult<Message[]>();
    let params = new HttpParams();
    params =  params.append("MessageContainer", messageContainer);
    if(page != null && itemsPerPage != null){
      params = params.append('pageNumber', page);
      params = params.append('pageSize', itemsPerPage);
      return this.http.get<Message[]>(`${this.baseUrl}user/${userId}/messages`,
        {observe: 'response', params})
        .pipe(
          map( response => {
            paginatedResult.result = response.body;
            if(response.headers.get('Pagination') !== null) {
              paginatedResult.pagination = JSON.parse(response.headers.get('pagination'));
            }
            return paginatedResult;
          })
        )
    }
  }

  getMessageThread(id: number, recipientId: number) {
    return this.http.get<Message[]>(`${this.baseUrl}user/${id}/messages/thread/${recipientId}`);
  }

  sendMessage(id: number, message: Message) {
    return this.http.post(`${this.baseUrl}user/${id}/messages`, message);
  }

  deleteMessage(id: number, messageId: number) {
    return this.http.delete(`${this.baseUrl}user/${id}/messages/${messageId}`, {});
  }

  markAsRead(id:number, messageId:number) {
    return this.http.post(`${this.baseUrl}user/${id}/messages/${messageId}/read`, {}).subscribe();
  }
}
