import { Component, OnInit } from '@angular/core';
import { ActivatedRoute } from '@angular/router';
import { ContactService } from '../../services/contact-service';
import { FormControl, FormGroup, Validators } from '@angular/forms';
import { UpdatePasswordForm } from '../../entities/update-password-form';

@Component({
  selector: 'app-update-password',
  templateUrl: './update-password.component.html',
  styleUrl: './update-password.component.css'
})
export class UpdatePasswordComponent implements OnInit {
  public contactForm!: FormGroup;
  private contactId: string | null = null;
  constructor(private route: ActivatedRoute,
    private contactServive: ContactService) { }

  ngOnInit(): void {
    this.contactId = this.route.snapshot.paramMap.get('id');
    this.contactForm = new FormGroup({
      password: new FormControl('', Validators.required),
      newPassword: new FormControl('', Validators.required),
    });
  }

  onSubmit() {
    if (this.contactId) {
      const form: UpdatePasswordForm = {
        id: this.contactId,
        password: this.contactForm.get('password')?.value,
        newPassword: this.contactForm.get('newPassword')?.value,
      };

      this.contactServive.updatePassword(form);
    }
  }
}
