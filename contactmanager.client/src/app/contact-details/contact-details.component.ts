import { HttpClient } from '@angular/common/http';
import { Component, OnInit } from '@angular/core';
import { ActivatedRoute, Router } from '@angular/router';
import { Contact } from '../../entities/contact';
import { UserForm } from '../../entities/user-form';
import { CategoryEnum } from '../../entities/category';
import { SubCategoryEnum } from '../../entities/sub-category';
import { FormControl, FormGroup, Validators } from '@angular/forms';
import { UpdateForm } from '../../entities/update-form';
import { ContactService } from '../../services/contact-service';
import { DateAdapter } from '@angular/material/core';

@Component({
  selector: 'app-contact-details',
  templateUrl: './contact-details.component.html',
  styleUrl: './contact-details.component.css'
})
export class ContactDetailsComponent implements OnInit {

  public contact: Contact | undefined = undefined;
  public categories = Object.values(CategoryEnum);
  public subCategories = Object.values(SubCategoryEnum);
  public selectedCategory: CategoryEnum | null = null;
  public selectedSubCategory: SubCategoryEnum | null = null
  public contactForm!: FormGroup;
  public contactId: string | null = null;
  constructor(private route: ActivatedRoute,
    private contactServive: ContactService, private adapter: DateAdapter<Date>) { }

  ngOnInit(): void {
    this.contactForm = new FormGroup({
      name: new FormControl('', Validators.required),
      surname: new FormControl('', Validators.required),
      category: new FormControl('', Validators.required),
      subCategory: new FormControl('', Validators.required),
      otherSubCategory: new FormControl('', Validators.required),
      phoneNumber: new FormControl('', Validators.required),
      email: new FormControl('', [Validators.required, Validators.email]),
      birthDate: new FormControl('', Validators.required)
    });
    this.contactId = this.route.snapshot.paramMap.get('id');
    if (this.contactId)
      this.getContact(this.contactId);
  }

   async getContact(id: string) {
    const userForm: UserForm = {
      id: id
     };
     this.contactServive.getContactById(userForm).subscribe(value => {
       this.contact = value;
       const date: Date | null = this.adapter.parse(value.birthDate, 'MM/DD/YYYY');
       this.contactForm.patchValue(value);
       this.contactForm.patchValue({ birthDate: date });
     })
  }

  onCategoryChange(category: CategoryEnum) {
    this.selectedCategory = category;
  }

  onSubCategoryChange(subCategory: SubCategoryEnum) {
    this.selectedSubCategory = subCategory;
  }

  public onSubmit() {
    if (this.contactId) {
      const updateForm: UpdateForm = {
        id: this.contactId,
        name: this.contactForm.get('name')!.value,
        surname: this.contactForm.get('surname')!.value,
        category: this.contactForm.get('category')!.value,
        subCategory: this.contactForm.get('subCategory')!.value,
        otherSubCategory: this.contactForm.get('otherSubCategory')!.value,
        phoneNumber: this.contactForm.get('phoneNumber')!.value,
        email: this.contactForm.get('email')!.value,
        birthDate: this.contactForm.get('birthDate')!.value,
      };

      this.contactServive.update(updateForm);
    } 
  }

  public onDelete() {
    if (this.contactId) {
      const form: UserForm = {
        id: this.contactId
      };

      this.contactServive.delete(form);
    }
  }

  get isBusinessCategory(): boolean {
    return this.selectedCategory === CategoryEnum.Business;
  }

  get isOtherSubCategory(): boolean {
    return this.selectedSubCategory === SubCategoryEnum.Other;
  }
}
