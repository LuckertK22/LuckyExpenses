export interface PagedResponse<T> {
  items: T[];
  totalItems: number;
  page: number;
  size: number;
}

export interface Paged<T> {
  items: T[];
  totalItems: number;
  page: number;
  size: number;
}