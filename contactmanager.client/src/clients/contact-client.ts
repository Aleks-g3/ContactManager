import { HttpClient } from "@angular/common/http";
import { Injectable } from "@angular/core";
import { Observable } from "rxjs";
import { SimpleContact } from "../entities/simple-contact";
import { UserForm } from "../entities/user-form";
import { Contact } from "../entities/contact";
import { LoginRequest } from "../entities/login-request";
import { RegisterForm } from "../entities/register-form";
import { UpdateForm } from "../entities/update-form";
import { UpdatePasswordForm } from "../entities/update-password-form";

@Injectable({
  providedIn: 'root',
})
export class ContactClient {
  constructor(private http: HttpClient) { }

  public login(loginRequest: LoginRequest): Observable<string> {
    return this.http.post('/login',
      JSON.stringify(loginRequest),
      { responseType: 'text' }
    );
  }

  public register(
    registerForm: RegisterForm
  ): Observable<string> {
    return this.http.post('/register',
      JSON.stringify(registerForm),
      { responseType: 'text' }
    );
  }

  public update(updateForm: UpdateForm): Observable<any> {
    return this.http.post('/update', JSON.stringify(updateForm), { responseType: 'text' });
  }

  public updatePassword(updatePasswordForm: UpdatePasswordForm): Observable<any> {
    return this.http.post('/updatePassword', JSON.stringify(updatePasswordForm), { responseType: 'text' });
  }

  getContacts(): Observable<SimpleContact[]> {
    return this.http.post<SimpleContact[]>('/getall', null);
  }

  getContactById(userForm: UserForm): Observable<Contact> {
    return this.http.post<Contact>('/getById', JSON.stringify(userForm))
  }

  delete(userForm: UserForm): Observable<any> {
    return this.http.post('/delete', JSON.stringify(userForm))
  }
}
