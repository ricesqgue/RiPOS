/**
 * Generated by orval v7.7.0 🍺
 * Do not edit manually.
 * RiPOS API
 * OpenAPI spec version: v1
 */
import type { CountryStateResponse } from './countryStateResponse';

export interface CustomerResponse {
  id?: number;
  /** @nullable */
  name?: string | null;
  /** @nullable */
  surname?: string | null;
  /** @nullable */
  secondSurname?: string | null;
  /** @nullable */
  phoneNumber?: string | null;
  /** @nullable */
  mobilePhone?: string | null;
  /** @nullable */
  email?: string | null;
  /** @nullable */
  address?: string | null;
  /** @nullable */
  city?: string | null;
  /** @nullable */
  zipCode?: string | null;
  /** @nullable */
  rfc?: string | null;
  countryState?: CountryStateResponse;
  isActive?: boolean;
}
