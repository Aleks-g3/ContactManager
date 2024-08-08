import { Component } from '@angular/core';
import { ContactService } from '../services/contact-service';


@Component({
  selector: 'app-root',
  templateUrl: './app.component.html',
  styleUrl: './app.component.css'
})
export class AppComponent {

  constructor(private contactService: ContactService) { }

  logout(): void {
    this.contactService.logout();
  }

  get isLoggedIn(): boolean {
    return this.contactService.isLoggedIn();
  }

  
  title = 'contactmanager.client';
}
