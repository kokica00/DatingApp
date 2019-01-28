import { Injectable } from '@angular/core';
import { HttpClient } from '@angular/common/http';
import { map } from 'rxjs/operators';
// najbolje je pro odmah sve oznaciti u app.modulu
@Injectable({
  providedIn: 'root'  // root predstavlja root modul... u ovom slucaju app.module.ts
})
export class AuthService {

baseUrl = 'http://localhost:5000/api/auth/';

constructor(private http: HttpClient) { }

login(model: any) {
  return this.http.post(this.baseUrl + 'login', model)
  .pipe(
    map((response: any) => {
      const user = response;
      if (user) {
        localStorage.setItem('token', user.token);
      }
    })
  );
}

register(model: any) {
  return this.http.post(this.baseUrl + 'register', model);
}

}
