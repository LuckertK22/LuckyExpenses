import { Injectable, inject } from '@angular/core';
import { HttpClient, HttpParams } from '@angular/common/http';
import { Observable, map } from 'rxjs';

import { environment } from '../../../environments/environment';
import {
  ApiResponse,
  GetDashboardSummaryQuery,
  GetDashboardSummaryResponse,
} from '../models';

@Injectable({ providedIn: 'root' })
export class DashboardService {
  private readonly http = inject(HttpClient);

  private readonly base = `${environment.apiUrl}/Dashboard`;

  getSummary(query?: GetDashboardSummaryQuery): Observable<GetDashboardSummaryResponse> {
    let params = new HttpParams();
    if (query?.year) {
      params = params.set('year', query.year);
    }
    if (query?.month) {
      params = params.set('month', query.month);
    }
    return this.http
      .get<ApiResponse<GetDashboardSummaryResponse>>(`${this.base}/Summary`, { params })
      .pipe(map((res) => res.data!));
  }
}
