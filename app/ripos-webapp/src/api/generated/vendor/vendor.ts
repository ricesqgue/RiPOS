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
  GetApiVendorsParams,
  StringMessageResponse,
  VendorRequest,
  VendorResponse,
  VendorResponseMessageResponse,
} from '.././models';

import getApiVendorsMutator from '../../axiosMutator';
import postApiVendorsMutator from '../../axiosMutator';
import getApiVendorsIdMutator from '../../axiosMutator';
import putApiVendorsIdMutator from '../../axiosMutator';
import deleteApiVendorsIdMutator from '../../axiosMutator';

export const getApiVendors = (params?: GetApiVendorsParams, signal?: AbortSignal) => {
  return getApiVendorsMutator<VendorResponse[]>({
    url: `/api/vendors`,
    method: 'GET',
    params,
    signal,
  });
};

export const getGetApiVendorsQueryKey = (params?: GetApiVendorsParams) => {
  return [`/api/vendors`, ...(params ? [params] : [])] as const;
};

export const getGetApiVendorsQueryOptions = <
  TData = Awaited<ReturnType<typeof getApiVendors>>,
  TError = void,
>(
  params?: GetApiVendorsParams,
  options?: {
    query?: Partial<UseQueryOptions<Awaited<ReturnType<typeof getApiVendors>>, TError, TData>>;
  }
) => {
  const { query: queryOptions } = options ?? {};

  const queryKey = queryOptions?.queryKey ?? getGetApiVendorsQueryKey(params);

  const queryFn: QueryFunction<Awaited<ReturnType<typeof getApiVendors>>> = ({ signal }) =>
    getApiVendors(params, signal);

  return { queryKey, queryFn, ...queryOptions } as UseQueryOptions<
    Awaited<ReturnType<typeof getApiVendors>>,
    TError,
    TData
  > & { queryKey: DataTag<QueryKey, TData, TError> };
};

export type GetApiVendorsQueryResult = NonNullable<Awaited<ReturnType<typeof getApiVendors>>>;
export type GetApiVendorsQueryError = void;

export function useGetApiVendors<TData = Awaited<ReturnType<typeof getApiVendors>>, TError = void>(
  params: undefined | GetApiVendorsParams,
  options: {
    query: Partial<UseQueryOptions<Awaited<ReturnType<typeof getApiVendors>>, TError, TData>> &
      Pick<
        DefinedInitialDataOptions<
          Awaited<ReturnType<typeof getApiVendors>>,
          TError,
          Awaited<ReturnType<typeof getApiVendors>>
        >,
        'initialData'
      >;
  }
): DefinedUseQueryResult<TData, TError> & { queryKey: DataTag<QueryKey, TData, TError> };
export function useGetApiVendors<TData = Awaited<ReturnType<typeof getApiVendors>>, TError = void>(
  params?: GetApiVendorsParams,
  options?: {
    query?: Partial<UseQueryOptions<Awaited<ReturnType<typeof getApiVendors>>, TError, TData>> &
      Pick<
        UndefinedInitialDataOptions<
          Awaited<ReturnType<typeof getApiVendors>>,
          TError,
          Awaited<ReturnType<typeof getApiVendors>>
        >,
        'initialData'
      >;
  }
): UseQueryResult<TData, TError> & { queryKey: DataTag<QueryKey, TData, TError> };
export function useGetApiVendors<TData = Awaited<ReturnType<typeof getApiVendors>>, TError = void>(
  params?: GetApiVendorsParams,
  options?: {
    query?: Partial<UseQueryOptions<Awaited<ReturnType<typeof getApiVendors>>, TError, TData>>;
  }
): UseQueryResult<TData, TError> & { queryKey: DataTag<QueryKey, TData, TError> };

export function useGetApiVendors<TData = Awaited<ReturnType<typeof getApiVendors>>, TError = void>(
  params?: GetApiVendorsParams,
  options?: {
    query?: Partial<UseQueryOptions<Awaited<ReturnType<typeof getApiVendors>>, TError, TData>>;
  }
): UseQueryResult<TData, TError> & { queryKey: DataTag<QueryKey, TData, TError> } {
  const queryOptions = getGetApiVendorsQueryOptions(params, options);

  const query = useQuery(queryOptions) as UseQueryResult<TData, TError> & {
    queryKey: DataTag<QueryKey, TData, TError>;
  };

  query.queryKey = queryOptions.queryKey;

  return query;
}

export const postApiVendors = (vendorRequest: VendorRequest, signal?: AbortSignal) => {
  return postApiVendorsMutator<VendorResponseMessageResponse>({
    url: `/api/vendors`,
    method: 'POST',
    headers: { 'Content-Type': 'application/json' },
    data: vendorRequest,
    signal,
  });
};

export const getPostApiVendorsMutationOptions = <TError = void, TContext = unknown>(options?: {
  mutation?: UseMutationOptions<
    Awaited<ReturnType<typeof postApiVendors>>,
    TError,
    { data: VendorRequest },
    TContext
  >;
}): UseMutationOptions<
  Awaited<ReturnType<typeof postApiVendors>>,
  TError,
  { data: VendorRequest },
  TContext
> => {
  const mutationKey = ['postApiVendors'];
  const { mutation: mutationOptions } = options
    ? options.mutation && 'mutationKey' in options.mutation && options.mutation.mutationKey
      ? options
      : { ...options, mutation: { ...options.mutation, mutationKey } }
    : { mutation: { mutationKey } };

  const mutationFn: MutationFunction<
    Awaited<ReturnType<typeof postApiVendors>>,
    { data: VendorRequest }
  > = (props) => {
    const { data } = props ?? {};

    return postApiVendors(data);
  };

  return { mutationFn, ...mutationOptions };
};

export type PostApiVendorsMutationResult = NonNullable<Awaited<ReturnType<typeof postApiVendors>>>;
export type PostApiVendorsMutationBody = VendorRequest;
export type PostApiVendorsMutationError = void;

export const usePostApiVendors = <TError = void, TContext = unknown>(options?: {
  mutation?: UseMutationOptions<
    Awaited<ReturnType<typeof postApiVendors>>,
    TError,
    { data: VendorRequest },
    TContext
  >;
}): UseMutationResult<
  Awaited<ReturnType<typeof postApiVendors>>,
  TError,
  { data: VendorRequest },
  TContext
> => {
  const mutationOptions = getPostApiVendorsMutationOptions(options);

  return useMutation(mutationOptions);
};
export const getApiVendorsId = (id: number, signal?: AbortSignal) => {
  return getApiVendorsIdMutator<VendorResponse>({
    url: `/api/vendors/${id}`,
    method: 'GET',
    signal,
  });
};

export const getGetApiVendorsIdQueryKey = (id: number) => {
  return [`/api/vendors/${id}`] as const;
};

export const getGetApiVendorsIdQueryOptions = <
  TData = Awaited<ReturnType<typeof getApiVendorsId>>,
  TError = void,
>(
  id: number,
  options?: {
    query?: Partial<UseQueryOptions<Awaited<ReturnType<typeof getApiVendorsId>>, TError, TData>>;
  }
) => {
  const { query: queryOptions } = options ?? {};

  const queryKey = queryOptions?.queryKey ?? getGetApiVendorsIdQueryKey(id);

  const queryFn: QueryFunction<Awaited<ReturnType<typeof getApiVendorsId>>> = ({ signal }) =>
    getApiVendorsId(id, signal);

  return { queryKey, queryFn, enabled: !!id, ...queryOptions } as UseQueryOptions<
    Awaited<ReturnType<typeof getApiVendorsId>>,
    TError,
    TData
  > & { queryKey: DataTag<QueryKey, TData, TError> };
};

export type GetApiVendorsIdQueryResult = NonNullable<Awaited<ReturnType<typeof getApiVendorsId>>>;
export type GetApiVendorsIdQueryError = void;

export function useGetApiVendorsId<
  TData = Awaited<ReturnType<typeof getApiVendorsId>>,
  TError = void,
>(
  id: number,
  options: {
    query: Partial<UseQueryOptions<Awaited<ReturnType<typeof getApiVendorsId>>, TError, TData>> &
      Pick<
        DefinedInitialDataOptions<
          Awaited<ReturnType<typeof getApiVendorsId>>,
          TError,
          Awaited<ReturnType<typeof getApiVendorsId>>
        >,
        'initialData'
      >;
  }
): DefinedUseQueryResult<TData, TError> & { queryKey: DataTag<QueryKey, TData, TError> };
export function useGetApiVendorsId<
  TData = Awaited<ReturnType<typeof getApiVendorsId>>,
  TError = void,
>(
  id: number,
  options?: {
    query?: Partial<UseQueryOptions<Awaited<ReturnType<typeof getApiVendorsId>>, TError, TData>> &
      Pick<
        UndefinedInitialDataOptions<
          Awaited<ReturnType<typeof getApiVendorsId>>,
          TError,
          Awaited<ReturnType<typeof getApiVendorsId>>
        >,
        'initialData'
      >;
  }
): UseQueryResult<TData, TError> & { queryKey: DataTag<QueryKey, TData, TError> };
export function useGetApiVendorsId<
  TData = Awaited<ReturnType<typeof getApiVendorsId>>,
  TError = void,
>(
  id: number,
  options?: {
    query?: Partial<UseQueryOptions<Awaited<ReturnType<typeof getApiVendorsId>>, TError, TData>>;
  }
): UseQueryResult<TData, TError> & { queryKey: DataTag<QueryKey, TData, TError> };

export function useGetApiVendorsId<
  TData = Awaited<ReturnType<typeof getApiVendorsId>>,
  TError = void,
>(
  id: number,
  options?: {
    query?: Partial<UseQueryOptions<Awaited<ReturnType<typeof getApiVendorsId>>, TError, TData>>;
  }
): UseQueryResult<TData, TError> & { queryKey: DataTag<QueryKey, TData, TError> } {
  const queryOptions = getGetApiVendorsIdQueryOptions(id, options);

  const query = useQuery(queryOptions) as UseQueryResult<TData, TError> & {
    queryKey: DataTag<QueryKey, TData, TError>;
  };

  query.queryKey = queryOptions.queryKey;

  return query;
}

export const putApiVendorsId = (id: number, vendorRequest: VendorRequest) => {
  return putApiVendorsIdMutator<VendorResponseMessageResponse>({
    url: `/api/vendors/${id}`,
    method: 'PUT',
    headers: { 'Content-Type': 'application/json' },
    data: vendorRequest,
  });
};

export const getPutApiVendorsIdMutationOptions = <TError = void, TContext = unknown>(options?: {
  mutation?: UseMutationOptions<
    Awaited<ReturnType<typeof putApiVendorsId>>,
    TError,
    { id: number; data: VendorRequest },
    TContext
  >;
}): UseMutationOptions<
  Awaited<ReturnType<typeof putApiVendorsId>>,
  TError,
  { id: number; data: VendorRequest },
  TContext
> => {
  const mutationKey = ['putApiVendorsId'];
  const { mutation: mutationOptions } = options
    ? options.mutation && 'mutationKey' in options.mutation && options.mutation.mutationKey
      ? options
      : { ...options, mutation: { ...options.mutation, mutationKey } }
    : { mutation: { mutationKey } };

  const mutationFn: MutationFunction<
    Awaited<ReturnType<typeof putApiVendorsId>>,
    { id: number; data: VendorRequest }
  > = (props) => {
    const { id, data } = props ?? {};

    return putApiVendorsId(id, data);
  };

  return { mutationFn, ...mutationOptions };
};

export type PutApiVendorsIdMutationResult = NonNullable<
  Awaited<ReturnType<typeof putApiVendorsId>>
>;
export type PutApiVendorsIdMutationBody = VendorRequest;
export type PutApiVendorsIdMutationError = void;

export const usePutApiVendorsId = <TError = void, TContext = unknown>(options?: {
  mutation?: UseMutationOptions<
    Awaited<ReturnType<typeof putApiVendorsId>>,
    TError,
    { id: number; data: VendorRequest },
    TContext
  >;
}): UseMutationResult<
  Awaited<ReturnType<typeof putApiVendorsId>>,
  TError,
  { id: number; data: VendorRequest },
  TContext
> => {
  const mutationOptions = getPutApiVendorsIdMutationOptions(options);

  return useMutation(mutationOptions);
};
export const deleteApiVendorsId = (id: number) => {
  return deleteApiVendorsIdMutator<StringMessageResponse>({
    url: `/api/vendors/${id}`,
    method: 'DELETE',
  });
};

export const getDeleteApiVendorsIdMutationOptions = <TError = void, TContext = unknown>(options?: {
  mutation?: UseMutationOptions<
    Awaited<ReturnType<typeof deleteApiVendorsId>>,
    TError,
    { id: number },
    TContext
  >;
}): UseMutationOptions<
  Awaited<ReturnType<typeof deleteApiVendorsId>>,
  TError,
  { id: number },
  TContext
> => {
  const mutationKey = ['deleteApiVendorsId'];
  const { mutation: mutationOptions } = options
    ? options.mutation && 'mutationKey' in options.mutation && options.mutation.mutationKey
      ? options
      : { ...options, mutation: { ...options.mutation, mutationKey } }
    : { mutation: { mutationKey } };

  const mutationFn: MutationFunction<
    Awaited<ReturnType<typeof deleteApiVendorsId>>,
    { id: number }
  > = (props) => {
    const { id } = props ?? {};

    return deleteApiVendorsId(id);
  };

  return { mutationFn, ...mutationOptions };
};

export type DeleteApiVendorsIdMutationResult = NonNullable<
  Awaited<ReturnType<typeof deleteApiVendorsId>>
>;

export type DeleteApiVendorsIdMutationError = void;

export const useDeleteApiVendorsId = <TError = void, TContext = unknown>(options?: {
  mutation?: UseMutationOptions<
    Awaited<ReturnType<typeof deleteApiVendorsId>>,
    TError,
    { id: number },
    TContext
  >;
}): UseMutationResult<
  Awaited<ReturnType<typeof deleteApiVendorsId>>,
  TError,
  { id: number },
  TContext
> => {
  const mutationOptions = getDeleteApiVendorsIdMutationOptions(options);

  return useMutation(mutationOptions);
};
