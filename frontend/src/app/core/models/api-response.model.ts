export interface ApiResponse<T> {
  ok: true;
  message: string;
  data: T | null;
  errors?: unknown;
}

export type ApiResponseData<T> = NonNullable<ApiResponse<T>['data']>;