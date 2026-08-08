import { Injectable, inject, signal } from '@angular/core';
import { HttpClient, HttpParams } from '@angular/common/http';
import { Observable, map } from 'rxjs';

import { environment } from '../../../environments/environment';
import {
  ApiResponse,
  GetCategoriesQuery,
  GetPaymentMethodsQuery,
  PagedResponse,
  ReferenceDto,
} from '../models';

@Injectable({ providedIn: 'root' })
export class ReferenceService {
  private readonly http = inject(HttpClient);

  private readonly _categories = signal<ReferenceDto[]>([]);
  readonly categories = this._categories.asReadonly();

  private readonly _paymentMethods = signal<ReferenceDto[]>([]);
  readonly paymentMethods = this._paymentMethods.asReadonly();

  private readonly base = `${environment.apiUrl}`;

  loadCategories(): Observable<ReferenceDto[]> {
    return this.http
      .get<ApiResponse<PagedResponse<ReferenceDto>>>(
        `${this.base}/Categories/GetCategories`,
        { params: { page: 1, size: 100 } },
      )
      .pipe(
        map((res) => res.data?.items ?? []),
        map((items) => {
          this._categories.set(items);
          return items;
        }),
      );
  }

  loadPaymentMethods(): Observable<ReferenceDto[]> {
    return this.http
      .get<ApiResponse<PagedResponse<ReferenceDto>>>(
        `${this.base}/PaymentMethods/GetPaymentMethods`,
        { params: { page: 1, size: 100 } },
      )
      .pipe(
        map((res) => res.data?.items ?? []),
        map((items) => {
          this._paymentMethods.set(items);
          return items;
        }),
      );
  }

  getCategories(
    query: GetCategoriesQuery,
  ): Observable<PagedResponse<ReferenceDto>> {
    return this.http
      .get<ApiResponse<PagedResponse<ReferenceDto>>>(
        `${this.base}/Categories/GetCategories`,
        { params: this.toParams(query) },
      )
      .pipe(map((res) => res.data!));
  }

  getPaymentMethods(
    query: GetPaymentMethodsQuery,
  ): Observable<PagedResponse<ReferenceDto>> {
    return this.http
      .get<ApiResponse<PagedResponse<ReferenceDto>>>(
        `${this.base}/PaymentMethods/GetPaymentMethods`,
        { params: this.toParams(query) },
      )
      .pipe(map((res) => res.data!));
  }

  private toParams(query: {
    search?: string;
    page: number;
    size: number;
  }): HttpParams {
    let params = new HttpParams().set('page', query.page).set('size', query.size);
    if (query.search) {
      params = params.set('search', query.search);
    }
    return params;
  }
}
