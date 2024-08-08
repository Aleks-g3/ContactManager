import { Router } from "@angular/router";
import { Injectable } from "@angular/core";
import { LoginRequest } from "../entities/login-request";
import { RegisterForm } from "../entities/register-form";
import { UpdateForm } from "../entities/update-form";
import { UpdatePasswordForm } from "../entities/update-password-form";
import { ContactClient } from "../clients/contact-client";
import { Observable } from "rxjs";
import { SimpleContact } from "../entities/simple-contact";
import { UserForm } from "../entities/user-form";
import { Contact } from "../entities/contact";

@Injectable({
  providedIn: 'root',
})
export class ContactService {
  private tokenKey = 'token';
  private contacts: SimpleContact[] = [];


  constructor(
    private contactClient: ContactClient,
    private router: Router
  ) { }

  public login(loginRequest: LoginRequest): void {
    this.contactClient.login(loginRequest).subscribe((token) => {
      var response = JSON.parse(token);
      localStorage.setItem(this.tokenKey, response.accessToken);
      this.router.navigate(['/']);
    });
  }

  public register(registerForm: RegisterForm): void {
    this.contactClient
      .register(registerForm)
      .subscribe((token) => {
        this.router.navigate(['/contacts']);
      });
  }

  public update(updateForm: UpdateForm): void {
    this.contactClient
      .update(updateForm)
      .subscribe();
    this.router.navigate(['/contacts']);
  }

  public updatePassword(updatePasswordForm: UpdatePasswordForm): void {
    this.contactClient
      .updatePassword(updatePasswordForm)
      .subscribe();
    this.router.navigate(['/contacts']);
  }

  public getContacts(): Observable<SimpleContact[]> {
    return this.contactClient.getContacts();
  }

  getContactById(userForm: UserForm): Observable<Contact> {
    return this.contactClient.getContactById(userForm)
  }

  delete(userForm: UserForm): void {
    this.contactClient.delete(userForm).subscribe();
    this.router.navigate(['/contacts']);
  }

  public logout() {
    localStorage.removeItem(this.tokenKey);
    this.router.navigate(['/contacts']);
  }

  public isLoggedIn(): boolean {
    let token = localStorage.getItem(this.tokenKey);
    return token != null && token.length > 0;
  }

  public getToken(): string | null {
    return this.isLoggedIn() ? localStorage.getItem(this.tokenKey) : null;
  }
}
