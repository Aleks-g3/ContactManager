import { Component } from '@angular/core';
import { SimpleContact } from '../../entities/simple-contact';
import { ContactService } from '../../services/contact-service';
import { Observable } from 'rxjs';

@Component({
  selector: 'app-contact-list',
  templateUrl: './contact-list.component.html',
  styleUrl: './contact-list.component.css'
})
export class ContactListComponent {

  public contacts$: Observable<SimpleContact[]> = this.contactService.getContacts();
  constructor(private contactService: ContactService) { }

  get isLoggedIn(): boolean {
    return this.contactService.isLoggedIn();
  }
}
