/**
 * Generated by orval v7.7.0 🍺
 * Do not edit manually.
 * RiPOS API
 * OpenAPI spec version: v1
 */
import { useQuery } from '@tanstack/react-query';
import type {
  DataTag,
  DefinedInitialDataOptions,
  DefinedUseQueryResult,
  QueryFunction,
  QueryKey,
  UndefinedInitialDataOptions,
  UseQueryOptions,
  UseQueryResult,
} from '@tanstack/react-query';

import type {
  GetApiStoreUsersParams,
  GetApiUsersParams,
  SimpleResponse,
  UserResponse,
} from '.././models';

import getApiUsersMutator from '../../axiosMutator';
import getApiStoreUsersMutator from '../../axiosMutator';
import getApiStoreUsersIdMutator from '../../axiosMutator';

export const getApiUsers = (params?: GetApiUsersParams, signal?: AbortSignal) => {
  return getApiUsersMutator<UserResponse[]>({ url: `/api/users`, method: 'GET', params, signal });
};

export const getGetApiUsersQueryKey = (params?: GetApiUsersParams) => {
  return [`/api/users`, ...(params ? [params] : [])] as const;
};

export const getGetApiUsersQueryOptions = <
  TData = Awaited<ReturnType<typeof getApiUsers>>,
  TError = void,
>(
  params?: GetApiUsersParams,
  options?: {
    query?: Partial<UseQueryOptions<Awaited<ReturnType<typeof getApiUsers>>, TError, TData>>;
  }
) => {
  const { query: queryOptions } = options ?? {};

  const queryKey = queryOptions?.queryKey ?? getGetApiUsersQueryKey(params);

  const queryFn: QueryFunction<Awaited<ReturnType<typeof getApiUsers>>> = ({ signal }) =>
    getApiUsers(params, signal);

  return { queryKey, queryFn, ...queryOptions } as UseQueryOptions<
    Awaited<ReturnType<typeof getApiUsers>>,
    TError,
    TData
  > & { queryKey: DataTag<QueryKey, TData, TError> };
};

export type GetApiUsersQueryResult = NonNullable<Awaited<ReturnType<typeof getApiUsers>>>;
export type GetApiUsersQueryError = void;

export function useGetApiUsers<TData = Awaited<ReturnType<typeof getApiUsers>>, TError = void>(
  params: undefined | GetApiUsersParams,
  options: {
    query: Partial<UseQueryOptions<Awaited<ReturnType<typeof getApiUsers>>, TError, TData>> &
      Pick<
        DefinedInitialDataOptions<
          Awaited<ReturnType<typeof getApiUsers>>,
          TError,
          Awaited<ReturnType<typeof getApiUsers>>
        >,
        'initialData'
      >;
  }
): DefinedUseQueryResult<TData, TError> & { queryKey: DataTag<QueryKey, TData, TError> };
export function useGetApiUsers<TData = Awaited<ReturnType<typeof getApiUsers>>, TError = void>(
  params?: GetApiUsersParams,
  options?: {
    query?: Partial<UseQueryOptions<Awaited<ReturnType<typeof getApiUsers>>, TError, TData>> &
      Pick<
        UndefinedInitialDataOptions<
          Awaited<ReturnType<typeof getApiUsers>>,
          TError,
          Awaited<ReturnType<typeof getApiUsers>>
        >,
        'initialData'
      >;
  }
): UseQueryResult<TData, TError> & { queryKey: DataTag<QueryKey, TData, TError> };
export function useGetApiUsers<TData = Awaited<ReturnType<typeof getApiUsers>>, TError = void>(
  params?: GetApiUsersParams,
  options?: {
    query?: Partial<UseQueryOptions<Awaited<ReturnType<typeof getApiUsers>>, TError, TData>>;
  }
): UseQueryResult<TData, TError> & { queryKey: DataTag<QueryKey, TData, TError> };

export function useGetApiUsers<TData = Awaited<ReturnType<typeof getApiUsers>>, TError = void>(
  params?: GetApiUsersParams,
  options?: {
    query?: Partial<UseQueryOptions<Awaited<ReturnType<typeof getApiUsers>>, TError, TData>>;
  }
): UseQueryResult<TData, TError> & { queryKey: DataTag<QueryKey, TData, TError> } {
  const queryOptions = getGetApiUsersQueryOptions(params, options);

  const query = useQuery(queryOptions) as UseQueryResult<TData, TError> & {
    queryKey: DataTag<QueryKey, TData, TError>;
  };

  query.queryKey = queryOptions.queryKey;

  return query;
}

export const getApiStoreUsers = (params?: GetApiStoreUsersParams, signal?: AbortSignal) => {
  return getApiStoreUsersMutator<UserResponse[]>({
    url: `/api/store/users`,
    method: 'GET',
    params,
    signal,
  });
};

export const getGetApiStoreUsersQueryKey = (params?: GetApiStoreUsersParams) => {
  return [`/api/store/users`, ...(params ? [params] : [])] as const;
};

export const getGetApiStoreUsersQueryOptions = <
  TData = Awaited<ReturnType<typeof getApiStoreUsers>>,
  TError = void,
>(
  params?: GetApiStoreUsersParams,
  options?: {
    query?: Partial<UseQueryOptions<Awaited<ReturnType<typeof getApiStoreUsers>>, TError, TData>>;
  }
) => {
  const { query: queryOptions } = options ?? {};

  const queryKey = queryOptions?.queryKey ?? getGetApiStoreUsersQueryKey(params);

  const queryFn: QueryFunction<Awaited<ReturnType<typeof getApiStoreUsers>>> = ({ signal }) =>
    getApiStoreUsers(params, signal);

  return { queryKey, queryFn, ...queryOptions } as UseQueryOptions<
    Awaited<ReturnType<typeof getApiStoreUsers>>,
    TError,
    TData
  > & { queryKey: DataTag<QueryKey, TData, TError> };
};

export type GetApiStoreUsersQueryResult = NonNullable<Awaited<ReturnType<typeof getApiStoreUsers>>>;
export type GetApiStoreUsersQueryError = void;

export function useGetApiStoreUsers<
  TData = Awaited<ReturnType<typeof getApiStoreUsers>>,
  TError = void,
>(
  params: undefined | GetApiStoreUsersParams,
  options: {
    query: Partial<UseQueryOptions<Awaited<ReturnType<typeof getApiStoreUsers>>, TError, TData>> &
      Pick<
        DefinedInitialDataOptions<
          Awaited<ReturnType<typeof getApiStoreUsers>>,
          TError,
          Awaited<ReturnType<typeof getApiStoreUsers>>
        >,
        'initialData'
      >;
  }
): DefinedUseQueryResult<TData, TError> & { queryKey: DataTag<QueryKey, TData, TError> };
export function useGetApiStoreUsers<
  TData = Awaited<ReturnType<typeof getApiStoreUsers>>,
  TError = void,
>(
  params?: GetApiStoreUsersParams,
  options?: {
    query?: Partial<UseQueryOptions<Awaited<ReturnType<typeof getApiStoreUsers>>, TError, TData>> &
      Pick<
        UndefinedInitialDataOptions<
          Awaited<ReturnType<typeof getApiStoreUsers>>,
          TError,
          Awaited<ReturnType<typeof getApiStoreUsers>>
        >,
        'initialData'
      >;
  }
): UseQueryResult<TData, TError> & { queryKey: DataTag<QueryKey, TData, TError> };
export function useGetApiStoreUsers<
  TData = Awaited<ReturnType<typeof getApiStoreUsers>>,
  TError = void,
>(
  params?: GetApiStoreUsersParams,
  options?: {
    query?: Partial<UseQueryOptions<Awaited<ReturnType<typeof getApiStoreUsers>>, TError, TData>>;
  }
): UseQueryResult<TData, TError> & { queryKey: DataTag<QueryKey, TData, TError> };

export function useGetApiStoreUsers<
  TData = Awaited<ReturnType<typeof getApiStoreUsers>>,
  TError = void,
>(
  params?: GetApiStoreUsersParams,
  options?: {
    query?: Partial<UseQueryOptions<Awaited<ReturnType<typeof getApiStoreUsers>>, TError, TData>>;
  }
): UseQueryResult<TData, TError> & { queryKey: DataTag<QueryKey, TData, TError> } {
  const queryOptions = getGetApiStoreUsersQueryOptions(params, options);

  const query = useQuery(queryOptions) as UseQueryResult<TData, TError> & {
    queryKey: DataTag<QueryKey, TData, TError>;
  };

  query.queryKey = queryOptions.queryKey;

  return query;
}

export const getApiStoreUsersId = (id: number, signal?: AbortSignal) => {
  return getApiStoreUsersIdMutator<UserResponse>({
    url: `/api/store/users/${id}`,
    method: 'GET',
    signal,
  });
};

export const getGetApiStoreUsersIdQueryKey = (id: number) => {
  return [`/api/store/users/${id}`] as const;
};

export const getGetApiStoreUsersIdQueryOptions = <
  TData = Awaited<ReturnType<typeof getApiStoreUsersId>>,
  TError = void | SimpleResponse,
>(
  id: number,
  options?: {
    query?: Partial<UseQueryOptions<Awaited<ReturnType<typeof getApiStoreUsersId>>, TError, TData>>;
  }
) => {
  const { query: queryOptions } = options ?? {};

  const queryKey = queryOptions?.queryKey ?? getGetApiStoreUsersIdQueryKey(id);

  const queryFn: QueryFunction<Awaited<ReturnType<typeof getApiStoreUsersId>>> = ({ signal }) =>
    getApiStoreUsersId(id, signal);

  return { queryKey, queryFn, enabled: !!id, ...queryOptions } as UseQueryOptions<
    Awaited<ReturnType<typeof getApiStoreUsersId>>,
    TError,
    TData
  > & { queryKey: DataTag<QueryKey, TData, TError> };
};

export type GetApiStoreUsersIdQueryResult = NonNullable<
  Awaited<ReturnType<typeof getApiStoreUsersId>>
>;
export type GetApiStoreUsersIdQueryError = void | SimpleResponse;

export function useGetApiStoreUsersId<
  TData = Awaited<ReturnType<typeof getApiStoreUsersId>>,
  TError = void | SimpleResponse,
>(
  id: number,
  options: {
    query: Partial<UseQueryOptions<Awaited<ReturnType<typeof getApiStoreUsersId>>, TError, TData>> &
      Pick<
        DefinedInitialDataOptions<
          Awaited<ReturnType<typeof getApiStoreUsersId>>,
          TError,
          Awaited<ReturnType<typeof getApiStoreUsersId>>
        >,
        'initialData'
      >;
  }
): DefinedUseQueryResult<TData, TError> & { queryKey: DataTag<QueryKey, TData, TError> };
export function useGetApiStoreUsersId<
  TData = Awaited<ReturnType<typeof getApiStoreUsersId>>,
  TError = void | SimpleResponse,
>(
  id: number,
  options?: {
    query?: Partial<
      UseQueryOptions<Awaited<ReturnType<typeof getApiStoreUsersId>>, TError, TData>
    > &
      Pick<
        UndefinedInitialDataOptions<
          Awaited<ReturnType<typeof getApiStoreUsersId>>,
          TError,
          Awaited<ReturnType<typeof getApiStoreUsersId>>
        >,
        'initialData'
      >;
  }
): UseQueryResult<TData, TError> & { queryKey: DataTag<QueryKey, TData, TError> };
export function useGetApiStoreUsersId<
  TData = Awaited<ReturnType<typeof getApiStoreUsersId>>,
  TError = void | SimpleResponse,
>(
  id: number,
  options?: {
    query?: Partial<UseQueryOptions<Awaited<ReturnType<typeof getApiStoreUsersId>>, TError, TData>>;
  }
): UseQueryResult<TData, TError> & { queryKey: DataTag<QueryKey, TData, TError> };

export function useGetApiStoreUsersId<
  TData = Awaited<ReturnType<typeof getApiStoreUsersId>>,
  TError = void | SimpleResponse,
>(
  id: number,
  options?: {
    query?: Partial<UseQueryOptions<Awaited<ReturnType<typeof getApiStoreUsersId>>, TError, TData>>;
  }
): UseQueryResult<TData, TError> & { queryKey: DataTag<QueryKey, TData, TError> } {
  const queryOptions = getGetApiStoreUsersIdQueryOptions(id, options);

  const query = useQuery(queryOptions) as UseQueryResult<TData, TError> & {
    queryKey: DataTag<QueryKey, TData, TError>;
  };

  query.queryKey = queryOptions.queryKey;

  return query;
}
