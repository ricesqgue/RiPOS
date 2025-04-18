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
  AuthRequest,
  LoginResponse,
  RefreshTokenRequest,
  SimpleResponse,
  UserResponse,
} from '.././models';

import postApiAuthMutator from '../../axiosMutator';
import postApiAuthRefreshTokenMutator from '../../axiosMutator';
import postApiAuthLogoutMutator from '../../axiosMutator';
import getApiAuthUserInfoMutator from '../../axiosMutator';

export const postApiAuth = (authRequest: AuthRequest, signal?: AbortSignal) => {
  return postApiAuthMutator<LoginResponse>({
    url: `/api/auth`,
    method: 'POST',
    headers: { 'Content-Type': 'application/json' },
    data: authRequest,
    signal,
  });
};

export const getPostApiAuthMutationOptions = <
  TError = SimpleResponse | void,
  TContext = unknown,
>(options?: {
  mutation?: UseMutationOptions<
    Awaited<ReturnType<typeof postApiAuth>>,
    TError,
    { data: AuthRequest },
    TContext
  >;
}): UseMutationOptions<
  Awaited<ReturnType<typeof postApiAuth>>,
  TError,
  { data: AuthRequest },
  TContext
> => {
  const mutationKey = ['postApiAuth'];
  const { mutation: mutationOptions } = options
    ? options.mutation && 'mutationKey' in options.mutation && options.mutation.mutationKey
      ? options
      : { ...options, mutation: { ...options.mutation, mutationKey } }
    : { mutation: { mutationKey } };

  const mutationFn: MutationFunction<
    Awaited<ReturnType<typeof postApiAuth>>,
    { data: AuthRequest }
  > = (props) => {
    const { data } = props ?? {};

    return postApiAuth(data);
  };

  return { mutationFn, ...mutationOptions };
};

export type PostApiAuthMutationResult = NonNullable<Awaited<ReturnType<typeof postApiAuth>>>;
export type PostApiAuthMutationBody = AuthRequest;
export type PostApiAuthMutationError = SimpleResponse | void;

export const usePostApiAuth = <TError = SimpleResponse | void, TContext = unknown>(options?: {
  mutation?: UseMutationOptions<
    Awaited<ReturnType<typeof postApiAuth>>,
    TError,
    { data: AuthRequest },
    TContext
  >;
}): UseMutationResult<
  Awaited<ReturnType<typeof postApiAuth>>,
  TError,
  { data: AuthRequest },
  TContext
> => {
  const mutationOptions = getPostApiAuthMutationOptions(options);

  return useMutation(mutationOptions);
};
export const postApiAuthRefreshToken = (
  refreshTokenRequest: RefreshTokenRequest,
  signal?: AbortSignal
) => {
  return postApiAuthRefreshTokenMutator<LoginResponse>({
    url: `/api/auth/refreshToken`,
    method: 'POST',
    headers: { 'Content-Type': 'application/json' },
    data: refreshTokenRequest,
    signal,
  });
};

export const getPostApiAuthRefreshTokenMutationOptions = <
  TError = SimpleResponse | void,
  TContext = unknown,
>(options?: {
  mutation?: UseMutationOptions<
    Awaited<ReturnType<typeof postApiAuthRefreshToken>>,
    TError,
    { data: RefreshTokenRequest },
    TContext
  >;
}): UseMutationOptions<
  Awaited<ReturnType<typeof postApiAuthRefreshToken>>,
  TError,
  { data: RefreshTokenRequest },
  TContext
> => {
  const mutationKey = ['postApiAuthRefreshToken'];
  const { mutation: mutationOptions } = options
    ? options.mutation && 'mutationKey' in options.mutation && options.mutation.mutationKey
      ? options
      : { ...options, mutation: { ...options.mutation, mutationKey } }
    : { mutation: { mutationKey } };

  const mutationFn: MutationFunction<
    Awaited<ReturnType<typeof postApiAuthRefreshToken>>,
    { data: RefreshTokenRequest }
  > = (props) => {
    const { data } = props ?? {};

    return postApiAuthRefreshToken(data);
  };

  return { mutationFn, ...mutationOptions };
};

export type PostApiAuthRefreshTokenMutationResult = NonNullable<
  Awaited<ReturnType<typeof postApiAuthRefreshToken>>
>;
export type PostApiAuthRefreshTokenMutationBody = RefreshTokenRequest;
export type PostApiAuthRefreshTokenMutationError = SimpleResponse | void;

export const usePostApiAuthRefreshToken = <
  TError = SimpleResponse | void,
  TContext = unknown,
>(options?: {
  mutation?: UseMutationOptions<
    Awaited<ReturnType<typeof postApiAuthRefreshToken>>,
    TError,
    { data: RefreshTokenRequest },
    TContext
  >;
}): UseMutationResult<
  Awaited<ReturnType<typeof postApiAuthRefreshToken>>,
  TError,
  { data: RefreshTokenRequest },
  TContext
> => {
  const mutationOptions = getPostApiAuthRefreshTokenMutationOptions(options);

  return useMutation(mutationOptions);
};
export const postApiAuthLogout = (signal?: AbortSignal) => {
  return postApiAuthLogoutMutator<void>({ url: `/api/auth/logout`, method: 'POST', signal });
};

export const getPostApiAuthLogoutMutationOptions = <TError = void, TContext = unknown>(options?: {
  mutation?: UseMutationOptions<
    Awaited<ReturnType<typeof postApiAuthLogout>>,
    TError,
    void,
    TContext
  >;
}): UseMutationOptions<Awaited<ReturnType<typeof postApiAuthLogout>>, TError, void, TContext> => {
  const mutationKey = ['postApiAuthLogout'];
  const { mutation: mutationOptions } = options
    ? options.mutation && 'mutationKey' in options.mutation && options.mutation.mutationKey
      ? options
      : { ...options, mutation: { ...options.mutation, mutationKey } }
    : { mutation: { mutationKey } };

  const mutationFn: MutationFunction<Awaited<ReturnType<typeof postApiAuthLogout>>, void> = () => {
    return postApiAuthLogout();
  };

  return { mutationFn, ...mutationOptions };
};

export type PostApiAuthLogoutMutationResult = NonNullable<
  Awaited<ReturnType<typeof postApiAuthLogout>>
>;

export type PostApiAuthLogoutMutationError = void;

export const usePostApiAuthLogout = <TError = void, TContext = unknown>(options?: {
  mutation?: UseMutationOptions<
    Awaited<ReturnType<typeof postApiAuthLogout>>,
    TError,
    void,
    TContext
  >;
}): UseMutationResult<Awaited<ReturnType<typeof postApiAuthLogout>>, TError, void, TContext> => {
  const mutationOptions = getPostApiAuthLogoutMutationOptions(options);

  return useMutation(mutationOptions);
};
export const getApiAuthUserInfo = (signal?: AbortSignal) => {
  return getApiAuthUserInfoMutator<UserResponse>({
    url: `/api/auth/userInfo`,
    method: 'GET',
    signal,
  });
};

export const getGetApiAuthUserInfoQueryKey = () => {
  return [`/api/auth/userInfo`] as const;
};

export const getGetApiAuthUserInfoQueryOptions = <
  TData = Awaited<ReturnType<typeof getApiAuthUserInfo>>,
  TError = void,
>(options?: {
  query?: Partial<UseQueryOptions<Awaited<ReturnType<typeof getApiAuthUserInfo>>, TError, TData>>;
}) => {
  const { query: queryOptions } = options ?? {};

  const queryKey = queryOptions?.queryKey ?? getGetApiAuthUserInfoQueryKey();

  const queryFn: QueryFunction<Awaited<ReturnType<typeof getApiAuthUserInfo>>> = ({ signal }) =>
    getApiAuthUserInfo(signal);

  return { queryKey, queryFn, ...queryOptions } as UseQueryOptions<
    Awaited<ReturnType<typeof getApiAuthUserInfo>>,
    TError,
    TData
  > & { queryKey: DataTag<QueryKey, TData, TError> };
};

export type GetApiAuthUserInfoQueryResult = NonNullable<
  Awaited<ReturnType<typeof getApiAuthUserInfo>>
>;
export type GetApiAuthUserInfoQueryError = void;

export function useGetApiAuthUserInfo<
  TData = Awaited<ReturnType<typeof getApiAuthUserInfo>>,
  TError = void,
>(options: {
  query: Partial<UseQueryOptions<Awaited<ReturnType<typeof getApiAuthUserInfo>>, TError, TData>> &
    Pick<
      DefinedInitialDataOptions<
        Awaited<ReturnType<typeof getApiAuthUserInfo>>,
        TError,
        Awaited<ReturnType<typeof getApiAuthUserInfo>>
      >,
      'initialData'
    >;
}): DefinedUseQueryResult<TData, TError> & { queryKey: DataTag<QueryKey, TData, TError> };
export function useGetApiAuthUserInfo<
  TData = Awaited<ReturnType<typeof getApiAuthUserInfo>>,
  TError = void,
>(options?: {
  query?: Partial<UseQueryOptions<Awaited<ReturnType<typeof getApiAuthUserInfo>>, TError, TData>> &
    Pick<
      UndefinedInitialDataOptions<
        Awaited<ReturnType<typeof getApiAuthUserInfo>>,
        TError,
        Awaited<ReturnType<typeof getApiAuthUserInfo>>
      >,
      'initialData'
    >;
}): UseQueryResult<TData, TError> & { queryKey: DataTag<QueryKey, TData, TError> };
export function useGetApiAuthUserInfo<
  TData = Awaited<ReturnType<typeof getApiAuthUserInfo>>,
  TError = void,
>(options?: {
  query?: Partial<UseQueryOptions<Awaited<ReturnType<typeof getApiAuthUserInfo>>, TError, TData>>;
}): UseQueryResult<TData, TError> & { queryKey: DataTag<QueryKey, TData, TError> };

export function useGetApiAuthUserInfo<
  TData = Awaited<ReturnType<typeof getApiAuthUserInfo>>,
  TError = void,
>(options?: {
  query?: Partial<UseQueryOptions<Awaited<ReturnType<typeof getApiAuthUserInfo>>, TError, TData>>;
}): UseQueryResult<TData, TError> & { queryKey: DataTag<QueryKey, TData, TError> } {
  const queryOptions = getGetApiAuthUserInfoQueryOptions(options);

  const query = useQuery(queryOptions) as UseQueryResult<TData, TError> & {
    queryKey: DataTag<QueryKey, TData, TError>;
  };

  query.queryKey = queryOptions.queryKey;

  return query;
}
