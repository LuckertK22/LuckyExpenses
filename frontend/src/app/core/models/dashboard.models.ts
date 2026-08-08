export interface GetDashboardSummaryQuery {
  year?: number;
  month?: number;
}

export interface CategoryBreakdownItem {
  categoryId: number;
  categoryName: string;
  amount: number;
  percentage: number;
}

export interface GetDashboardSummaryResponse {
  year: number;
  month: number;
  totalAmount: number;
  totalExpenses: number;
  averageAmount: number;
  previousTotalAmount: number;
  changePercent: number;
  byCategory: CategoryBreakdownItem[];
}