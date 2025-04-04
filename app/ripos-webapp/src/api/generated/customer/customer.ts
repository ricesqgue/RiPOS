/**
 * Generated by orval v7.7.0 🍺
 * Do not edit manually.
 * RiPOS API
 * OpenAPI spec version: v1
 */
import { useMutation, useQuery } from '@tanstack/react-query';
import type {
  DataTag,
  DefinedInitialDataOptions,
  DefinedUseQueryResult,
  MutationFunction,
  QueryFunction,
  QueryKey,
  UndefinedInitialDataOptions,
  UseMutationOptions,
  UseMutationResult,
  UseQueryOptions,
  UseQueryResult,
} from '@tanstack/react-query';

import type {
  CustomerRequest,
  GetApiCustomersParams,
  StoreResponse,
  StoreResponseMessageResponse,
  StringMessageResponse,
} from '.././models';

import getApiCustomersMutator from '../../axiosMutator';
import postApiCustomersMutator from '../../axiosMutator';
import getApiCustomersIdMutator from '../../axiosMutator';
import putApiCustomersIdMutator from '../../axiosMutator';
import deleteApiCustomersIdMutator from '../../axiosMutator';

export const getApiCustomers = (params?: GetApiCustomersParams, signal?: AbortSignal) => {
  return getApiCustomersMutator<StoreResponse[]>({
    url: `/api/customers`,
    method: 'GET',
    params,
    signal,
  });
};

export const getGetApiCustomersQueryKey = (params?: GetApiCustomersParams) => {
  return [`/api/customers`, ...(params ? [params] : [])] as const;
};

export const getGetApiCustomersQueryOptions = <
  TData = Awaited<ReturnType<typeof getApiCustomers>>,
  TError = void,
>(
  params?: GetApiCustomersParams,
  options?: {
    query?: Partial<UseQueryOptions<Awaited<ReturnType<typeof getApiCustomers>>, TError, TData>>;
  }
) => {
  const { query: queryOptions } = options ?? {};

  const queryKey = queryOptions?.queryKey ?? getGetApiCustomersQueryKey(params);

  const queryFn: QueryFunction<Awaited<ReturnType<typeof getApiCustomers>>> = ({ signal }) =>
    getApiCustomers(params, signal);

  return { queryKey, queryFn, ...queryOptions } as UseQueryOptions<
    Awaited<ReturnType<typeof getApiCustomers>>,
    TError,
    TData
  > & { queryKey: DataTag<QueryKey, TData, TError> };
};

export type GetApiCustomersQueryResult = NonNullable<Awaited<ReturnType<typeof getApiCustomers>>>;
export type GetApiCustomersQueryError = void;

export function useGetApiCustomers<
  TData = Awaited<ReturnType<typeof getApiCustomers>>,
  TError = void,
>(
  params: undefined | GetApiCustomersParams,
  options: {
    query: Partial<UseQueryOptions<Awaited<ReturnType<typeof getApiCustomers>>, TError, TData>> &
      Pick<
        DefinedInitialDataOptions<
          Awaited<ReturnType<typeof getApiCustomers>>,
          TError,
          Awaited<ReturnType<typeof getApiCustomers>>
        >,
        'initialData'
      >;
  }
): DefinedUseQueryResult<TData, TError> & { queryKey: DataTag<QueryKey, TData, TError> };
export function useGetApiCustomers<
  TData = Awaited<ReturnType<typeof getApiCustomers>>,
  TError = void,
>(
  params?: GetApiCustomersParams,
  options?: {
    query?: Partial<UseQueryOptions<Awaited<ReturnType<typeof getApiCustomers>>, TError, TData>> &
      Pick<
        UndefinedInitialDataOptions<
          Awaited<ReturnType<typeof getApiCustomers>>,
          TError,
          Awaited<ReturnType<typeof getApiCustomers>>
        >,
        'initialData'
      >;
  }
): UseQueryResult<TData, TError> & { queryKey: DataTag<QueryKey, TData, TError> };
export function useGetApiCustomers<
  TData = Awaited<ReturnType<typeof getApiCustomers>>,
  TError = void,
>(
  params?: GetApiCustomersParams,
  options?: {
    query?: Partial<UseQueryOptions<Awaited<ReturnType<typeof getApiCustomers>>, TError, TData>>;
  }
): UseQueryResult<TData, TError> & { queryKey: DataTag<QueryKey, TData, TError> };

export function useGetApiCustomers<
  TData = Awaited<ReturnType<typeof getApiCustomers>>,
  TError = void,
>(
  params?: GetApiCustomersParams,
  options?: {
    query?: Partial<UseQueryOptions<Awaited<ReturnType<typeof getApiCustomers>>, TError, TData>>;
  }
): UseQueryResult<TData, TError> & { queryKey: DataTag<QueryKey, TData, TError> } {
  const queryOptions = getGetApiCustomersQueryOptions(params, options);

  const query = useQuery(queryOptions) as UseQueryResult<TData, TError> & {
    queryKey: DataTag<QueryKey, TData, TError>;
  };

  query.queryKey = queryOptions.queryKey;

  return query;
}

export const postApiCustomers = (customerRequest: CustomerRequest, signal?: AbortSignal) => {
  return postApiCustomersMutator<StoreResponseMessageResponse>({
    url: `/api/customers`,
    method: 'POST',
    headers: { 'Content-Type': 'application/json' },
    data: customerRequest,
    signal,
  });
};

export const getPostApiCustomersMutationOptions = <TError = void, TContext = unknown>(options?: {
  mutation?: UseMutationOptions<
    Awaited<ReturnType<typeof postApiCustomers>>,
    TError,
    { data: CustomerRequest },
    TContext
  >;
}): UseMutationOptions<
  Awaited<ReturnType<typeof postApiCustomers>>,
  TError,
  { data: CustomerRequest },
  TContext
> => {
  const mutationKey = ['postApiCustomers'];
  const { mutation: mutationOptions } = options
    ? options.mutation && 'mutationKey' in options.mutation && options.mutation.mutationKey
      ? options
      : { ...options, mutation: { ...options.mutation, mutationKey } }
    : { mutation: { mutationKey } };

  const mutationFn: MutationFunction<
    Awaited<ReturnType<typeof postApiCustomers>>,
    { data: CustomerRequest }
  > = (props) => {
    const { data } = props ?? {};

    return postApiCustomers(data);
  };

  return { mutationFn, ...mutationOptions };
};

export type PostApiCustomersMutationResult = NonNullable<
  Awaited<ReturnType<typeof postApiCustomers>>
>;
export type PostApiCustomersMutationBody = CustomerRequest;
export type PostApiCustomersMutationError = void;

export const usePostApiCustomers = <TError = void, TContext = unknown>(options?: {
  mutation?: UseMutationOptions<
    Awaited<ReturnType<typeof postApiCustomers>>,
    TError,
    { data: CustomerRequest },
    TContext
  >;
}): UseMutationResult<
  Awaited<ReturnType<typeof postApiCustomers>>,
  TError,
  { data: CustomerRequest },
  TContext
> => {
  const mutationOptions = getPostApiCustomersMutationOptions(options);

  return useMutation(mutationOptions);
};
export const getApiCustomersId = (id: number, signal?: AbortSignal) => {
  return getApiCustomersIdMutator<StoreResponse>({
    url: `/api/customers/${id}`,
    method: 'GET',
    signal,
  });
};

export const getGetApiCustomersIdQueryKey = (id: number) => {
  return [`/api/customers/${id}`] as const;
};

export const getGetApiCustomersIdQueryOptions = <
  TData = Awaited<ReturnType<typeof getApiCustomersId>>,
  TError = void,
>(
  id: number,
  options?: {
    query?: Partial<UseQueryOptions<Awaited<ReturnType<typeof getApiCustomersId>>, TError, TData>>;
  }
) => {
  const { query: queryOptions } = options ?? {};

  const queryKey = queryOptions?.queryKey ?? getGetApiCustomersIdQueryKey(id);

  const queryFn: QueryFunction<Awaited<ReturnType<typeof getApiCustomersId>>> = ({ signal }) =>
    getApiCustomersId(id, signal);

  return { queryKey, queryFn, enabled: !!id, ...queryOptions } as UseQueryOptions<
    Awaited<ReturnType<typeof getApiCustomersId>>,
    TError,
    TData
  > & { queryKey: DataTag<QueryKey, TData, TError> };
};

export type GetApiCustomersIdQueryResult = NonNullable<
  Awaited<ReturnType<typeof getApiCustomersId>>
>;
export type GetApiCustomersIdQueryError = void;

export function useGetApiCustomersId<
  TData = Awaited<ReturnType<typeof getApiCustomersId>>,
  TError = void,
>(
  id: number,
  options: {
    query: Partial<UseQueryOptions<Awaited<ReturnType<typeof getApiCustomersId>>, TError, TData>> &
      Pick<
        DefinedInitialDataOptions<
          Awaited<ReturnType<typeof getApiCustomersId>>,
          TError,
          Awaited<ReturnType<typeof getApiCustomersId>>
        >,
        'initialData'
      >;
  }
): DefinedUseQueryResult<TData, TError> & { queryKey: DataTag<QueryKey, TData, TError> };
export function useGetApiCustomersId<
  TData = Awaited<ReturnType<typeof getApiCustomersId>>,
  TError = void,
>(
  id: number,
  options?: {
    query?: Partial<UseQueryOptions<Awaited<ReturnType<typeof getApiCustomersId>>, TError, TData>> &
      Pick<
        UndefinedInitialDataOptions<
          Awaited<ReturnType<typeof getApiCustomersId>>,
          TError,
          Awaited<ReturnType<typeof getApiCustomersId>>
        >,
        'initialData'
      >;
  }
): UseQueryResult<TData, TError> & { queryKey: DataTag<QueryKey, TData, TError> };
export function useGetApiCustomersId<
  TData = Awaited<ReturnType<typeof getApiCustomersId>>,
  TError = void,
>(
  id: number,
  options?: {
    query?: Partial<UseQueryOptions<Awaited<ReturnType<typeof getApiCustomersId>>, TError, TData>>;
  }
): UseQueryResult<TData, TError> & { queryKey: DataTag<QueryKey, TData, TError> };

export function useGetApiCustomersId<
  TData = Awaited<ReturnType<typeof getApiCustomersId>>,
  TError = void,
>(
  id: number,
  options?: {
    query?: Partial<UseQueryOptions<Awaited<ReturnType<typeof getApiCustomersId>>, TError, TData>>;
  }
): UseQueryResult<TData, TError> & { queryKey: DataTag<QueryKey, TData, TError> } {
  const queryOptions = getGetApiCustomersIdQueryOptions(id, options);

  const query = useQuery(queryOptions) as UseQueryResult<TData, TError> & {
    queryKey: DataTag<QueryKey, TData, TError>;
  };

  query.queryKey = queryOptions.queryKey;

  return query;
}

export const putApiCustomersId = (id: number, customerRequest: CustomerRequest) => {
  return putApiCustomersIdMutator<StoreResponseMessageResponse>({
    url: `/api/customers/${id}`,
    method: 'PUT',
    headers: { 'Content-Type': 'application/json' },
    data: customerRequest,
  });
};

export const getPutApiCustomersIdMutationOptions = <TError = void, TContext = unknown>(options?: {
  mutation?: UseMutationOptions<
    Awaited<ReturnType<typeof putApiCustomersId>>,
    TError,
    { id: number; data: CustomerRequest },
    TContext
  >;
}): UseMutationOptions<
  Awaited<ReturnType<typeof putApiCustomersId>>,
  TError,
  { id: number; data: CustomerRequest },
  TContext
> => {
  const mutationKey = ['putApiCustomersId'];
  const { mutation: mutationOptions } = options
    ? options.mutation && 'mutationKey' in options.mutation && options.mutation.mutationKey
      ? options
      : { ...options, mutation: { ...options.mutation, mutationKey } }
    : { mutation: { mutationKey } };

  const mutationFn: MutationFunction<
    Awaited<ReturnType<typeof putApiCustomersId>>,
    { id: number; data: CustomerRequest }
  > = (props) => {
    const { id, data } = props ?? {};

    return putApiCustomersId(id, data);
  };

  return { mutationFn, ...mutationOptions };
};

export type PutApiCustomersIdMutationResult = NonNullable<
  Awaited<ReturnType<typeof putApiCustomersId>>
>;
export type PutApiCustomersIdMutationBody = CustomerRequest;
export type PutApiCustomersIdMutationError = void;

export const usePutApiCustomersId = <TError = void, TContext = unknown>(options?: {
  mutation?: UseMutationOptions<
    Awaited<ReturnType<typeof putApiCustomersId>>,
    TError,
    { id: number; data: CustomerRequest },
    TContext
  >;
}): UseMutationResult<
  Awaited<ReturnType<typeof putApiCustomersId>>,
  TError,
  { id: number; data: CustomerRequest },
  TContext
> => {
  const mutationOptions = getPutApiCustomersIdMutationOptions(options);

  return useMutation(mutationOptions);
};
export const deleteApiCustomersId = (id: number) => {
  return deleteApiCustomersIdMutator<StringMessageResponse>({
    url: `/api/customers/${id}`,
    method: 'DELETE',
  });
};

export const getDeleteApiCustomersIdMutationOptions = <
  TError = void,
  TContext = unknown,
>(options?: {
  mutation?: UseMutationOptions<
    Awaited<ReturnType<typeof deleteApiCustomersId>>,
    TError,
    { id: number },
    TContext
  >;
}): UseMutationOptions<
  Awaited<ReturnType<typeof deleteApiCustomersId>>,
  TError,
  { id: number },
  TContext
> => {
  const mutationKey = ['deleteApiCustomersId'];
  const { mutation: mutationOptions } = options
    ? options.mutation && 'mutationKey' in options.mutation && options.mutation.mutationKey
      ? options
      : { ...options, mutation: { ...options.mutation, mutationKey } }
    : { mutation: { mutationKey } };

  const mutationFn: MutationFunction<
    Awaited<ReturnType<typeof deleteApiCustomersId>>,
    { id: number }
  > = (props) => {
    const { id } = props ?? {};

    return deleteApiCustomersId(id);
  };

  return { mutationFn, ...mutationOptions };
};

export type DeleteApiCustomersIdMutationResult = NonNullable<
  Awaited<ReturnType<typeof deleteApiCustomersId>>
>;

export type DeleteApiCustomersIdMutationError = void;

export const useDeleteApiCustomersId = <TError = void, TContext = unknown>(options?: {
  mutation?: UseMutationOptions<
    Awaited<ReturnType<typeof deleteApiCustomersId>>,
    TError,
    { id: number },
    TContext
  >;
}): UseMutationResult<
  Awaited<ReturnType<typeof deleteApiCustomersId>>,
  TError,
  { id: number },
  TContext
> => {
  const mutationOptions = getDeleteApiCustomersIdMutationOptions(options);

  return useMutation(mutationOptions);
};
