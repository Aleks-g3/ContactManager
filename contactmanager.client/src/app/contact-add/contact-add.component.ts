import { Component, OnInit } from '@angular/core';
import { ContactService } from '../../services/contact-service';
import { FormControl, FormGroup, Validators } from '@angular/forms';
import { RegisterForm } from '../../entities/register-form';
import { SubCategoryEnum } from '../../entities/sub-category';
import { CategoryEnum } from '../../entities/category';

@Component({
  selector: 'app-contact-add',
  templateUrl: './contact-add.component.html',
  styleUrl: './contact-add.component.css'
})
export class ContactAddComponent implements OnInit {
  public categories = Object.values(CategoryEnum);
  public subCategories = Object.values(SubCategoryEnum);
  public selectedCategory: CategoryEnum | null = null;
  public selectedSubCategory: SubCategoryEnum | null = null
  public contactForm!: FormGroup;
  constructor(private contactService: ContactService) { }

    ngOnInit(): void {
      this.contactForm = new FormGroup({
        name: new FormControl('', Validators.required),
        surname: new FormControl('', Validators.required),
        category: new FormControl('', Validators.required),
        subCategory: new FormControl(''),
        otherSubCategory: new FormControl(''),
        phoneNumber: new FormControl('', Validators.required),
        email: new FormControl('', [Validators.required, Validators.email]),
        password: new FormControl('', Validators.required),
        birthDate: new FormControl('', Validators.required)
      });
  }

  onCategoryChange(category: CategoryEnum) {
    this.selectedCategory = category;
    this.selectedSubCategory = null;
    this.contactForm.patchValue({ otherSubCategory: null });
  }

  onSubCategoryChange(subCategory: SubCategoryEnum) {
    this.selectedSubCategory = subCategory;
  }

  public onSubmit() {
    const registerForm: RegisterForm = {
      name: this.contactForm.get('name')?.value,
      surname: this.contactForm.get('surname')?.value,
      category: this.contactForm.get('category')?.value,
      subCategory: this.contactForm.get('subCategory')?.value,
      otherSubCategory: this.contactForm.get('otherSubCategory')?.value,
      phoneNumber: this.contactForm.get('phoneNumber')?.value,
      email: this.contactForm.get('email')?.value,
      password: this.contactForm.get('password')?.value,
      birthDate: this.contactForm.get('birthDate')?.value,
    };

    this.contactService.register(registerForm);
  }

  get isBusinessCategory(): boolean {
    return this.selectedCategory === CategoryEnum.Business;
  }

  get isOtherSubCategory(): boolean {
    return this.selectedSubCategory === SubCategoryEnum.Other;
  }
}
