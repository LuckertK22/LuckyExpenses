import { Injectable, inject } from '@angular/core';
import { HttpClient, HttpParams } from '@angular/common/http';
import { Observable, map } from 'rxjs';

import { environment } from '../../../environments/environment';
import {
  ApiResponse,
  CreateExpenseCommand,
  CreateExpenseResponse,
  DeleteExpenseCommand,
  GetExpensesQuery,
  GetExpensesResponse,
  PagedResponse,
  UpdateExpenseCommand,
} from '../models';

@Injectable({ providedIn: 'root' })
export class ExpenseService {
  private readonly http = inject(HttpClient);

  private readonly base = `${environment.apiUrl}/Expenses`;

  create(command: CreateExpenseCommand): Observable<CreateExpenseResponse> {
    return this.http
      .post<ApiResponse<CreateExpenseResponse>>(`${this.base}/Create`, command)
      .pipe(map((res) => res.data!));
  }

  update(command: UpdateExpenseCommand): Observable<CreateExpenseResponse> {
    return this.http
      .put<ApiResponse<CreateExpenseResponse>>(`${this.base}/Update`, command)
      .pipe(map((res) => res.data!));
  }

  delete(command: DeleteExpenseCommand): Observable<void> {
    return this.http
      .request<void>('DELETE', `${this.base}/Delete`, { body: command })
      .pipe(map(() => undefined));
  }

  getExpenses(query: GetExpensesQuery): Observable<PagedResponse<GetExpensesResponse>> {
    const params = this.toParams(query);
    return this.http
      .get<ApiResponse<PagedResponse<GetExpensesResponse>>>(
        `${this.base}/GetExpenses`,
        { params },
      )
      .pipe(map((res) => res.data!));
  }

  private toParams(query: GetExpensesQuery): HttpParams {
    let params = new HttpParams()
      .set('page', query.page)
      .set('size', query.size);
    if (query.fromDate) {
      params = params.set('fromDate', query.fromDate);
    }
    if (query.toDate) {
      params = params.set('toDate', query.toDate);
    }
    if (query.categoryId) {
      params = params.set('categoryId', query.categoryId);
    }
    if (query.paymentMethodId) {
      params = params.set('paymentMethodId', query.paymentMethodId);
    }
    if (query.title) {
      params = params.set('title', query.title);
    }
    return params;
  }
}
