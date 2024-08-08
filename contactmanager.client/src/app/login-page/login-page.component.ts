import { Component, OnInit } from '@angular/core';
import { ContactService } from '../../services/contact-service';
import { FormControl, FormGroup, Validators } from '@angular/forms';
import { LoginRequest } from '../../entities/login-request';

@Component({
  selector: 'app-login-page',
  templateUrl: './login-page.component.html',
  styleUrl: './login-page.component.css'
})
export class LoginPageComponent implements OnInit {
  public loginForm!: FormGroup;

  constructor(private contactService: ContactService) { }

    ngOnInit(): void {
      this.loginForm = new FormGroup({
        email: new FormControl('', [Validators.required, Validators.email]),
        password: new FormControl('', Validators.required),
      });
    }

  public onSubmit() {
    const loginRequest: LoginRequest = {
      email: this.loginForm.get('email')!.value,
      password: this.loginForm!.get('password')!.value
    }
    this.contactService.login(loginRequest);
  }

}
