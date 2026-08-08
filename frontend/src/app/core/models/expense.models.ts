export interface CreateExpenseCommand {
  categoryId: number;
  paymentMethodId?: number | null;
  title: string;
  description?: string | null;
  amount: number;
  expenseDate: string;
}

export interface UpdateExpenseCommand {
  id: number;
  categoryId: number;
  paymentMethodId?: number | null;
  title: string;
  description?: string | null;
  amount: number;
  expenseDate: string;
}

export interface DeleteExpenseCommand {
  id: number;
}

export interface GetExpensesQuery {
  fromDate?: string;
  toDate?: string;
  categoryId?: number;
  paymentMethodId?: number;
  title?: string;
  page: number;
  size: number;
}

export interface GetExpenseByIdQuery {
  id: number;
}

export interface ExpenseDto {
  id: number;
  categoryId: number;
  paymentMethodId?: number | null;
  title: string;
  description?: string | null;
  amount: number;
  expenseDate: string;
  createdAt: string;
}

export interface ExpenseWithNamesDto extends ExpenseDto {
  categoryName: string;
  paymentMethodName?: string | null;
}

export type CreateExpenseResponse = ExpenseDto;
export type UpdateExpenseResponse = ExpenseDto;
export type GetExpensesResponse = ExpenseWithNamesDto;
export type GetExpenseByIdResponse = ExpenseWithNamesDto;