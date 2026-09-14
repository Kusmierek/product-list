export type AlertState =
  | { type: 'success'; message: string }
  | { type: 'error'; message: string }
  | null;

export type ProductSortBy = 'CreatedAt' | 'Name' | 'Price';
export type SortDirection = 'Asc' | 'Desc';

export interface ProductQuery {
  page: number;
  pageSize: number;
  sortBy: ProductSortBy;
  sortDir: SortDirection;
  search?: string;
}

export interface PagedResult<T> {
  items: T[];
  total: number;
  page: number;
  pageSize: number;
}

export interface Product {
  id: string;
  code: string;
  name: string;
  price: number;
  createdAt: string;
}

export interface CreateProductDto {
  code: string;
  name: string;
  price: number;
}
