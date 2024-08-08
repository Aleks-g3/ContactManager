import { CategoryEnum } from "./category";
import { SubCategoryEnum } from "./sub-category";

export interface Contact {
  id: string;
  name: string;
  surname: string;
  category: CategoryEnum;
  subCategory: SubCategoryEnum | null;
  otherSubCategory: string | null;
  birthDate: string;
  email: string;
  phoneNumber: string;
}
