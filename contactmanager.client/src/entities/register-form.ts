import { CategoryEnum } from "./category";
import { SubCategoryEnum } from "./sub-category";

export interface RegisterForm {
  email: string;
  password: string;
  name: string;
  surname: string;
  phoneNumber: string;
  category: CategoryEnum;
  subCategory: SubCategoryEnum | null;
  otherSubCategory: string | null;
  birthDate: string;
}
