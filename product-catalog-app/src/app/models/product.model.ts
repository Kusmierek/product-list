export type AlertState =
  | { type: 'success'; message: string }
  | { type: 'error'; message: string }
  | null;

export interface Product {
  id: string;
  code: string;
  name: string;
  price: number;
}

export interface CreateProductDto {
  code: string;
  name: string;
  price: number;
}
