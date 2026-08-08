export interface GetCategoriesQuery {
  search?: string;
  page: number;
  size: number;
}

export interface GetPaymentMethodsQuery {
  search?: string;
  page: number;
  size: number;
}

export interface ReferenceDto {
  id: number;
  code: string;
  name: string;
  createdAt: string;
}

export type GetCategoriesResponse = ReferenceDto;
export type GetPaymentMethodsResponse = ReferenceDto;