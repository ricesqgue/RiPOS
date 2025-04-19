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
  GetApiSizesParams,
  SimpleResponse,
  SizeRequest,
  SizeResponse,
  SizeResponseMessageResponse,
} from '.././models';

import getApiSizesMutator from '../../axiosMutator';
import postApiSizesMutator from '../../axiosMutator';
import getApiSizesIdMutator from '../../axiosMutator';
import putApiSizesIdMutator from '../../axiosMutator';
import deleteApiSizesIdMutator from '../../axiosMutator';

export const getApiSizes = (params?: GetApiSizesParams, signal?: AbortSignal) => {
  return getApiSizesMutator<SizeResponse[]>({ url: `/api/sizes`, method: 'GET', params, signal });
};

export const getGetApiSizesQueryKey = (params?: GetApiSizesParams) => {
  return [`/api/sizes`, ...(params ? [params] : [])] as const;
};

export const getGetApiSizesQueryOptions = <
  TData = Awaited<ReturnType<typeof getApiSizes>>,
  TError = void,
>(
  params?: GetApiSizesParams,
  options?: {
    query?: Partial<UseQueryOptions<Awaited<ReturnType<typeof getApiSizes>>, TError, TData>>;
  }
) => {
  const { query: queryOptions } = options ?? {};

  const queryKey = queryOptions?.queryKey ?? getGetApiSizesQueryKey(params);

  const queryFn: QueryFunction<Awaited<ReturnType<typeof getApiSizes>>> = ({ signal }) =>
    getApiSizes(params, signal);

  return { queryKey, queryFn, ...queryOptions } as UseQueryOptions<
    Awaited<ReturnType<typeof getApiSizes>>,
    TError,
    TData
  > & { queryKey: DataTag<QueryKey, TData, TError> };
};

export type GetApiSizesQueryResult = NonNullable<Awaited<ReturnType<typeof getApiSizes>>>;
export type GetApiSizesQueryError = void;

export function useGetApiSizes<TData = Awaited<ReturnType<typeof getApiSizes>>, TError = void>(
  params: undefined | GetApiSizesParams,
  options: {
    query: Partial<UseQueryOptions<Awaited<ReturnType<typeof getApiSizes>>, TError, TData>> &
      Pick<
        DefinedInitialDataOptions<
          Awaited<ReturnType<typeof getApiSizes>>,
          TError,
          Awaited<ReturnType<typeof getApiSizes>>
        >,
        'initialData'
      >;
  }
): DefinedUseQueryResult<TData, TError> & { queryKey: DataTag<QueryKey, TData, TError> };
export function useGetApiSizes<TData = Awaited<ReturnType<typeof getApiSizes>>, TError = void>(
  params?: GetApiSizesParams,
  options?: {
    query?: Partial<UseQueryOptions<Awaited<ReturnType<typeof getApiSizes>>, TError, TData>> &
      Pick<
        UndefinedInitialDataOptions<
          Awaited<ReturnType<typeof getApiSizes>>,
          TError,
          Awaited<ReturnType<typeof getApiSizes>>
        >,
        'initialData'
      >;
  }
): UseQueryResult<TData, TError> & { queryKey: DataTag<QueryKey, TData, TError> };
export function useGetApiSizes<TData = Awaited<ReturnType<typeof getApiSizes>>, TError = void>(
  params?: GetApiSizesParams,
  options?: {
    query?: Partial<UseQueryOptions<Awaited<ReturnType<typeof getApiSizes>>, TError, TData>>;
  }
): UseQueryResult<TData, TError> & { queryKey: DataTag<QueryKey, TData, TError> };

export function useGetApiSizes<TData = Awaited<ReturnType<typeof getApiSizes>>, TError = void>(
  params?: GetApiSizesParams,
  options?: {
    query?: Partial<UseQueryOptions<Awaited<ReturnType<typeof getApiSizes>>, TError, TData>>;
  }
): UseQueryResult<TData, TError> & { queryKey: DataTag<QueryKey, TData, TError> } {
  const queryOptions = getGetApiSizesQueryOptions(params, options);

  const query = useQuery(queryOptions) as UseQueryResult<TData, TError> & {
    queryKey: DataTag<QueryKey, TData, TError>;
  };

  query.queryKey = queryOptions.queryKey;

  return query;
}

export const postApiSizes = (sizeRequest: SizeRequest, signal?: AbortSignal) => {
  return postApiSizesMutator<SizeResponseMessageResponse>({
    url: `/api/sizes`,
    method: 'POST',
    headers: { 'Content-Type': 'application/json' },
    data: sizeRequest,
    signal,
  });
};

export const getPostApiSizesMutationOptions = <
  TError = SimpleResponse | void,
  TContext = unknown,
>(options?: {
  mutation?: UseMutationOptions<
    Awaited<ReturnType<typeof postApiSizes>>,
    TError,
    { data: SizeRequest },
    TContext
  >;
}): UseMutationOptions<
  Awaited<ReturnType<typeof postApiSizes>>,
  TError,
  { data: SizeRequest },
  TContext
> => {
  const mutationKey = ['postApiSizes'];
  const { mutation: mutationOptions } = options
    ? options.mutation && 'mutationKey' in options.mutation && options.mutation.mutationKey
      ? options
      : { ...options, mutation: { ...options.mutation, mutationKey } }
    : { mutation: { mutationKey } };

  const mutationFn: MutationFunction<
    Awaited<ReturnType<typeof postApiSizes>>,
    { data: SizeRequest }
  > = (props) => {
    const { data } = props ?? {};

    return postApiSizes(data);
  };

  return { mutationFn, ...mutationOptions };
};

export type PostApiSizesMutationResult = NonNullable<Awaited<ReturnType<typeof postApiSizes>>>;
export type PostApiSizesMutationBody = SizeRequest;
export type PostApiSizesMutationError = SimpleResponse | void;

export const usePostApiSizes = <TError = SimpleResponse | void, TContext = unknown>(options?: {
  mutation?: UseMutationOptions<
    Awaited<ReturnType<typeof postApiSizes>>,
    TError,
    { data: SizeRequest },
    TContext
  >;
}): UseMutationResult<
  Awaited<ReturnType<typeof postApiSizes>>,
  TError,
  { data: SizeRequest },
  TContext
> => {
  const mutationOptions = getPostApiSizesMutationOptions(options);

  return useMutation(mutationOptions);
};
export const getApiSizesId = (id: number, signal?: AbortSignal) => {
  return getApiSizesIdMutator<SizeResponse>({ url: `/api/sizes/${id}`, method: 'GET', signal });
};

export const getGetApiSizesIdQueryKey = (id: number) => {
  return [`/api/sizes/${id}`] as const;
};

export const getGetApiSizesIdQueryOptions = <
  TData = Awaited<ReturnType<typeof getApiSizesId>>,
  TError = void | SimpleResponse,
>(
  id: number,
  options?: {
    query?: Partial<UseQueryOptions<Awaited<ReturnType<typeof getApiSizesId>>, TError, TData>>;
  }
) => {
  const { query: queryOptions } = options ?? {};

  const queryKey = queryOptions?.queryKey ?? getGetApiSizesIdQueryKey(id);

  const queryFn: QueryFunction<Awaited<ReturnType<typeof getApiSizesId>>> = ({ signal }) =>
    getApiSizesId(id, signal);

  return { queryKey, queryFn, enabled: !!id, ...queryOptions } as UseQueryOptions<
    Awaited<ReturnType<typeof getApiSizesId>>,
    TError,
    TData
  > & { queryKey: DataTag<QueryKey, TData, TError> };
};

export type GetApiSizesIdQueryResult = NonNullable<Awaited<ReturnType<typeof getApiSizesId>>>;
export type GetApiSizesIdQueryError = void | SimpleResponse;

export function useGetApiSizesId<
  TData = Awaited<ReturnType<typeof getApiSizesId>>,
  TError = void | SimpleResponse,
>(
  id: number,
  options: {
    query: Partial<UseQueryOptions<Awaited<ReturnType<typeof getApiSizesId>>, TError, TData>> &
      Pick<
        DefinedInitialDataOptions<
          Awaited<ReturnType<typeof getApiSizesId>>,
          TError,
          Awaited<ReturnType<typeof getApiSizesId>>
        >,
        'initialData'
      >;
  }
): DefinedUseQueryResult<TData, TError> & { queryKey: DataTag<QueryKey, TData, TError> };
export function useGetApiSizesId<
  TData = Awaited<ReturnType<typeof getApiSizesId>>,
  TError = void | SimpleResponse,
>(
  id: number,
  options?: {
    query?: Partial<UseQueryOptions<Awaited<ReturnType<typeof getApiSizesId>>, TError, TData>> &
      Pick<
        UndefinedInitialDataOptions<
          Awaited<ReturnType<typeof getApiSizesId>>,
          TError,
          Awaited<ReturnType<typeof getApiSizesId>>
        >,
        'initialData'
      >;
  }
): UseQueryResult<TData, TError> & { queryKey: DataTag<QueryKey, TData, TError> };
export function useGetApiSizesId<
  TData = Awaited<ReturnType<typeof getApiSizesId>>,
  TError = void | SimpleResponse,
>(
  id: number,
  options?: {
    query?: Partial<UseQueryOptions<Awaited<ReturnType<typeof getApiSizesId>>, TError, TData>>;
  }
): UseQueryResult<TData, TError> & { queryKey: DataTag<QueryKey, TData, TError> };

export function useGetApiSizesId<
  TData = Awaited<ReturnType<typeof getApiSizesId>>,
  TError = void | SimpleResponse,
>(
  id: number,
  options?: {
    query?: Partial<UseQueryOptions<Awaited<ReturnType<typeof getApiSizesId>>, TError, TData>>;
  }
): UseQueryResult<TData, TError> & { queryKey: DataTag<QueryKey, TData, TError> } {
  const queryOptions = getGetApiSizesIdQueryOptions(id, options);

  const query = useQuery(queryOptions) as UseQueryResult<TData, TError> & {
    queryKey: DataTag<QueryKey, TData, TError>;
  };

  query.queryKey = queryOptions.queryKey;

  return query;
}

export const putApiSizesId = (id: number, sizeRequest: SizeRequest) => {
  return putApiSizesIdMutator<SizeResponseMessageResponse>({
    url: `/api/sizes/${id}`,
    method: 'PUT',
    headers: { 'Content-Type': 'application/json' },
    data: sizeRequest,
  });
};

export const getPutApiSizesIdMutationOptions = <
  TError = SimpleResponse | void,
  TContext = unknown,
>(options?: {
  mutation?: UseMutationOptions<
    Awaited<ReturnType<typeof putApiSizesId>>,
    TError,
    { id: number; data: SizeRequest },
    TContext
  >;
}): UseMutationOptions<
  Awaited<ReturnType<typeof putApiSizesId>>,
  TError,
  { id: number; data: SizeRequest },
  TContext
> => {
  const mutationKey = ['putApiSizesId'];
  const { mutation: mutationOptions } = options
    ? options.mutation && 'mutationKey' in options.mutation && options.mutation.mutationKey
      ? options
      : { ...options, mutation: { ...options.mutation, mutationKey } }
    : { mutation: { mutationKey } };

  const mutationFn: MutationFunction<
    Awaited<ReturnType<typeof putApiSizesId>>,
    { id: number; data: SizeRequest }
  > = (props) => {
    const { id, data } = props ?? {};

    return putApiSizesId(id, data);
  };

  return { mutationFn, ...mutationOptions };
};

export type PutApiSizesIdMutationResult = NonNullable<Awaited<ReturnType<typeof putApiSizesId>>>;
export type PutApiSizesIdMutationBody = SizeRequest;
export type PutApiSizesIdMutationError = SimpleResponse | void;

export const usePutApiSizesId = <TError = SimpleResponse | void, TContext = unknown>(options?: {
  mutation?: UseMutationOptions<
    Awaited<ReturnType<typeof putApiSizesId>>,
    TError,
    { id: number; data: SizeRequest },
    TContext
  >;
}): UseMutationResult<
  Awaited<ReturnType<typeof putApiSizesId>>,
  TError,
  { id: number; data: SizeRequest },
  TContext
> => {
  const mutationOptions = getPutApiSizesIdMutationOptions(options);

  return useMutation(mutationOptions);
};
export const deleteApiSizesId = (id: number) => {
  return deleteApiSizesIdMutator<SimpleResponse>({ url: `/api/sizes/${id}`, method: 'DELETE' });
};

export const getDeleteApiSizesIdMutationOptions = <
  TError = SimpleResponse | void,
  TContext = unknown,
>(options?: {
  mutation?: UseMutationOptions<
    Awaited<ReturnType<typeof deleteApiSizesId>>,
    TError,
    { id: number },
    TContext
  >;
}): UseMutationOptions<
  Awaited<ReturnType<typeof deleteApiSizesId>>,
  TError,
  { id: number },
  TContext
> => {
  const mutationKey = ['deleteApiSizesId'];
  const { mutation: mutationOptions } = options
    ? options.mutation && 'mutationKey' in options.mutation && options.mutation.mutationKey
      ? options
      : { ...options, mutation: { ...options.mutation, mutationKey } }
    : { mutation: { mutationKey } };

  const mutationFn: MutationFunction<
    Awaited<ReturnType<typeof deleteApiSizesId>>,
    { id: number }
  > = (props) => {
    const { id } = props ?? {};

    return deleteApiSizesId(id);
  };

  return { mutationFn, ...mutationOptions };
};

export type DeleteApiSizesIdMutationResult = NonNullable<
  Awaited<ReturnType<typeof deleteApiSizesId>>
>;

export type DeleteApiSizesIdMutationError = SimpleResponse | void;

export const useDeleteApiSizesId = <TError = SimpleResponse | void, TContext = unknown>(options?: {
  mutation?: UseMutationOptions<
    Awaited<ReturnType<typeof deleteApiSizesId>>,
    TError,
    { id: number },
    TContext
  >;
}): UseMutationResult<
  Awaited<ReturnType<typeof deleteApiSizesId>>,
  TError,
  { id: number },
  TContext
> => {
  const mutationOptions = getDeleteApiSizesIdMutationOptions(options);

  return useMutation(mutationOptions);
};
